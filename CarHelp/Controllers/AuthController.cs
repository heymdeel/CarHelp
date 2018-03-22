using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CarHelp.BLL.Model.DTO;
using CarHelp.BLL.Services;
using CarHelp.DAL.Entities;
using CarHelp.Options;
using CarHelp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CarHelp.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class AuthController : Controller
    {
        private readonly IAccountService accountService;

        public AuthController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        // POST: api/sms_code
        [HttpPost("sms_code")]
        public async Task<IActionResult> GetSmsCode([FromBody]string phone)
        {
            if (!accountService.ValidatePhone(phone))
            {
                return BadRequest();
            }

            await accountService.GenerateSmsCodeAsync(phone);

            return Ok();
        }

        // POST: api/users
        [HttpPost("users")]
        public async Task<IActionResult> SignUpUser([FromBody]UserSignUpDTO userData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await accountService.UserExistsAsync(userData.Phone))
            {
                return BadRequest("user already exists");
            }

            User user = await accountService.SignUpUserAsync(userData);
            if (user == null)
            {
                return NotFound();
            }

            var tokenVM = await GetTokenVMAsync(user);

            return Ok(tokenVM);
        }

        // POST: api/sign_in
        [HttpPost("sign_in")]
        public async Task<IActionResult> SignInUser([FromBody]UserSignInDTO userData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = await accountService.SignInUserAsync(userData);
            if (user == null)
            {
                return NotFound();
            }

            var tokenVM = await GetTokenVMAsync(user);

            return Ok(tokenVM);
        }

        // POST: api/token
        [HttpPost("token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            if (!ValidateRerfreshToken(refreshToken))
            {
                return NotFound();
            }

            int userId = int.Parse(new JwtSecurityTokenHandler().ReadJwtToken(refreshToken).Claims.FirstOrDefault(c => c.Type == "user_id").Value);
            
            User user = await accountService.FindUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var tokenVM = await GetTokenVMAsync(user);

            return Ok(tokenVM);
        }

        // DELETE: api/token
        [HttpDelete("token")]
        public async Task<IActionResult> InvalidateToken([FromBody] string refreshToken)
        {
            if (!ValidateRerfreshToken(refreshToken))
            {
                return NotFound();
            }

            int userId = int.Parse(new JwtSecurityTokenHandler().ReadJwtToken(refreshToken).Claims.FirstOrDefault(c => c.Type == "user_id").Value);

            await accountService.InvalidateTokenAsync(userId, refreshToken);

            return NoContent(); ;
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
            // TODO: change security algorithm
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: tokenType == TokenType.Access ? AuthOptions.ACCESS_AUDIENCE : AuthOptions.REFRESH_AUDIENCE,
                    notBefore: DateTime.Now,
                    claims: identity.Claims,
                    expires: DateTime.Now.AddMinutes(tokenType == TokenType.Refresh ? AuthOptions.REFRESH_LIFETIME : AuthOptions.ACCESS_LIFETIME),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private async Task<TokenVM> GetTokenVMAsync(User user)
        {
            ClaimsIdentity identity = GetIdentity(user);

            string refreshToken = GenerateToken(identity, TokenType.Refresh);
            string accessToken = GenerateToken(identity, TokenType.Access);

            await accountService.SaveUserTokenAsync(user, refreshToken);
            user.RefreshToken = refreshToken;

            var tokenVM = Mapper.Map<TokenVM>(user);
            tokenVM.AccessToken = accessToken;

            return tokenVM;
        }

        private bool ValidateRerfreshToken(string refreshToken)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = AuthOptions.ISSUER,

                ValidateAudience = true,
                ValidAudience = AuthOptions.REFRESH_AUDIENCE,

                ValidateLifetime = true,

                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
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
    }
}