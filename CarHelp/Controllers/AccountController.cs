using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarHelp.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarHelp.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet("code/{phone}")]
        public async Task<IActionResult> GetSmsCode([FromRoute]string phone)
        {
            if (!accountService.ValidatePhone(phone))
            {
                return BadRequest();
            }

            await accountService.GenerateSmsCode(phone);

            return Ok();
        }
    }
}