using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CarHelp.AppLayer.Models.DTO;
using CarHelp.AppLayer.Services;
using CarHelp.DAL.Entities;
using CarHelp.Options;
using CarHelp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CarHelp.Controllers
{
    [Produces("application/json")]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IAccountService accountService;
        private readonly ISmsService smsService;
        private readonly AuthOptions authOptions;

        public AuthController(IAccountService accountService, ISmsService smsService, IOptions<AuthOptions> options)
        {
            this.authOptions = options.Value;
            this.accountService = accountService;
            this.smsService = smsService;
        }

        // GET: api/auth/sms_code
        /// <summary> Request for sending code via sms</summary>
        /// <response code="400"> bad phone format </response>
        [HttpGet("sms_code")]
        public async Task<IActionResult> GetSmsCode([FromQuery]string phone)
        {
            if (!smsService.ValidatePhone(phone))
            {
                return BadRequest("bad phone format");
            }

            await smsService.SendCodeAsync(phone);

            return Ok();
        }

        // POST: api/auth/sing_up
        /// <summary> Sign up user </summary>
        /// <response code="400"> invalid code, errors in model validation or user already exists </response>
        /// <response code="200"> tokens and user's roles </response>
        [HttpPost("sign_up")]
        [ProducesResponseType(typeof(TokenVM), 200)]
        public async Task<IActionResult> SignUpUser([FromBody]UserSignUpDTO userData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await smsService.CodeIsValidAsync(userData.Phone, userData.SmsCode))
            {
                return BadRequest("invalid code");
            }

            if (await accountService.UserExistsAsync(userData.Phone))
            {
                return BadRequest("user already exists");
            }

            User user = await accountService.SignUpUserAsync(userData);
            var tokenVM = await GetTokenVMAsync(user);

            return Ok(tokenVM);
        }

        // POST: api/auth/sign_in
        /// <summary> Sign in user </summary>
        /// <response code="200"> tokens and user's roles </response>
        /// <response code="400"> invalid code or errors in model validation </response>
        /// <response code="404"> user wasn't found </response>
        [HttpPost("sign_in")]
        [ProducesResponseType(typeof(TokenVM), 200)]
        public async Task<IActionResult> SignInUser([FromBody]UserSignInDTO userData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await smsService.CodeIsValidAsync(userData.Phone, userData.SmsCode))
            {
                return BadRequest("invalid code");
            }

            User user = await accountService.SignInUserAsync(userData);
            if (user == null)
            {
                return NotFound();
            }
            
            var tokenVM = await GetTokenVMAsync(user);

            return Ok(tokenVM);
        }

        // POST: api/auth/token
        /// <summary> Get new access and refresh token </summary>
        /// <response code="200"> tokens and user's roles </response>
        /// <response code="400"> invalid refresh token </response>
        /// <response code="404"> user with this token wasn't found </response>
        [HttpPost("token")]
        [ProducesResponseType(typeof(TokenVM), 200)]
        public async Task<IActionResult> RefreshTokens([FromBody] string refreshToken)
        {
            if (!ValidateRerfreshToken(refreshToken))
            {
                return BadRequest("invalid token. need to sign in");
            }

            int userId = GetUserIdFromToken(refreshToken);

            User user = await accountService.FindUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var tokenVM = await GetTokenVMAsync(user);

            return Ok(tokenVM);
        }

        // DELETE: api/auth/token
        /// <summary> Invalidate refresh token </summary>
        /// <responce code="400"> invalid refresh token </responce>
        [HttpDelete("token")]
        public async Task<IActionResult> InvalidateToken([FromBody] string refreshToken)
        {
            if (!ValidateRerfreshToken(refreshToken))
            {
                return BadRequest("invalid token");
            }

            int userId = GetUserIdFromToken(refreshToken);

            await accountService.InvalidateTokenAsync(userId, refreshToken);

            return NoContent();
        }

        private ClaimsIdentity GetIdentity(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("user_id", user.Id.ToString())
            };

            claims.AddRange(user.Roles.Select(r => new Claim(ClaimsIdentity.DefaultRoleClaimType, r)));

            return new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }
        
        private string GenerateToken(ClaimsIdentity identity, TokenType tokenType)
        {
            Console.WriteLine(authOptions.Key);
            
            var jwt = new JwtSecurityToken(
                    issuer: authOptions.Issuer,
                    audience: tokenType == TokenType.Refresh ? authOptions.RefreshAudience : authOptions.AccessAudience,
                    notBefore: DateTime.Now,
                    claims: identity.Claims,
                    expires: DateTime.Now.AddMinutes(tokenType == TokenType.Refresh ? AuthOptions.REFRESH_LIFETIME : AuthOptions.ACCESS_LIFETIME),
                    signingCredentials: new SigningCredentials(authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private async Task<TokenVM> GetTokenVMAsync(User user)
        {
            ClaimsIdentity identity = GetIdentity(user);

            string refreshToken = GenerateToken(identity, TokenType.Refresh);
            string accessToken = GenerateToken(identity, TokenType.Access);

            await accountService.StoreRefreshTokenAsync(user, refreshToken);

            var tokenVM = Mapper.Map<TokenVM>(user);
            tokenVM.AccessToken = accessToken;
            tokenVM.RefreshToken = refreshToken;

            return tokenVM;
        }

        private bool ValidateRerfreshToken(string refreshToken)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = authOptions.Issuer,

                ValidateAudience = true,
                ValidAudience = authOptions.RefreshAudience,

                ValidateLifetime = true,

                IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                ValidateIssuerSigningKey = true,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken = null;
            try
            {
                tokenHandler.ValidateToken(refreshToken, validationParameters, out validatedToken);
            }
            catch (SecurityTokenException)
            {
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        private int GetUserIdFromToken(string refreshToken) => int.Parse(new JwtSecurityTokenHandler().ReadJwtToken(refreshToken).Claims.FirstOrDefault(c => c.Type == "user_id").Value);
    }
}