using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CarHelp.AppLayer.Models;
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
    [Route("api")]
    public class AuthController : Controller
    {
        private readonly IAuthService authService;
        private readonly ISmsService smsService;

        public AuthController(IAuthService authService, ISmsService smsService)
        {
            this.authService = authService;
            this.smsService = smsService;
        }

        // GET: api/sms_code?phone
        /// <summary> Request for sending code via sms </summary>
        /// <response code="200"> success </response>
        /// <response code="400"> bad phone format </response>
        [HttpGet("sms_code")]
        public async Task<IActionResult> GetSmsCode([FromQuery]string phone)
        {
            if (phone == null || !smsService.PhoneNumberIsValid(phone))
            {
                return BadRequest("bad phone format");
            }

            await smsService.SendCodeAsync(phone);

            return Ok();
        }

        // POST: api/sing_up
        /// <summary> Sign up user </summary>
        /// <response code="200"> tokens and user's roles </response>
        /// <response code="400"> invalid code, errors in model validation or user already exists </response>
        [HttpPost("sign_up")]
        [ProducesResponseType(typeof(TokenVM), 200)]
        public async Task<IActionResult> SignUpUser([FromBody]SignUpInput userData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = await authService.SignUpUserAsync(userData);
            var tokens = await authService.GenerateAndStoreTokensAsync(user);

            var tokenVM = Mapper.Map<TokenVM>((tokens, user));

            return Ok(tokenVM);
        }

        // POST: api/sign_in
        /// <summary> Sign in user </summary>
        /// <response code="200"> tokens and user's roles </response>
        /// <response code="400"> invalid code, errors in model validation or user was not found </response>
        [HttpPost("sign_in")]
        [ProducesResponseType(typeof(TokenVM), 200)]
        public async Task<IActionResult> SignInUser([FromBody]SignInInput userData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = await authService.SignInUserAsync(userData);
            var tokens = await authService.GenerateAndStoreTokensAsync(user);

            var tokenVM = Mapper.Map<TokenVM>((tokens, user));

            return Ok(tokenVM);
        }

        // POST: api/token
        /// <summary> Get new access and refresh token </summary>
        /// <response code="200"> tokens and user's roles </response>
        /// <response code="400"> invalid refresh token or user with this id wasn't found </response>
        [HttpPost("token")]
        [ProducesResponseType(typeof(TokenVM), 200)]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var tokens = await authService.RefreshTokenAsync(refreshToken);

            var tokenVM = Mapper.Map<TokenVM>(tokens);

            return Ok(tokenVM);
        }

        // DELETE: api/token
        /// <summary> Invalidate refresh token </summary>
        /// <responce code="204"> Token was invalidated</responce>
        /// <responce code="400"> invalid refresh token or user doen't exist </responce>
        [HttpDelete("token")]
        public async Task<IActionResult> InvalidateToken([FromBody] string refreshToken)
        {
            await authService.InvalidateTokenAsync(refreshToken);

            return NoContent();
        }
    }
}