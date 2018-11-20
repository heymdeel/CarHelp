using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CarHelp.AppLayer.Models.DTO;
using CarHelp.AppLayer.Services;
using CarHelp.DAL.Entities;
using CarHelp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CarHelp.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}")]
    [Produces("application/json")]
    public class AuthController : Controller
    {
        private readonly IAuthService authService;
        private readonly ISmsService smsService;
        private readonly IMapper mapper;

        public AuthController(IAuthService authService, ISmsService smsService, IMapper mapper)
        {
            this.authService = authService;
            this.smsService = smsService;
            this.mapper = mapper;
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

            var tokenVM = mapper.Map<TokenVM>((tokens, user));

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

            var tokenVM = mapper.Map<TokenVM>((tokens, user));

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

            var tokenVM = mapper.Map<TokenVM>(tokens);

            return Ok(tokenVM);
        }

        // DELETE: api/token
        /// <summary> Invalidate refresh token </summary>
        /// <response code="204"> Token was invalidated</response>
        /// <response code="400"> invalid refresh token or user doen't exist </response>
        [HttpDelete("token")]
        public async Task<IActionResult> InvalidateToken([FromBody] string refreshToken)
        {
            await authService.InvalidateTokenAsync(refreshToken);

            return NoContent();
        }
    }
}