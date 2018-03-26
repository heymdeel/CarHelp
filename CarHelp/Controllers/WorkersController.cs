using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarHelp.BLL.Model.DTO;
using CarHelp.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarHelp.Controllers
{
    [Produces("application/json")]
    [Route("api/workers")]
    public class WorkersController : Controller
    {
        private readonly IWorkersService workersService;
        private readonly IOrdersService ordersService;

        public WorkersController(IWorkersService workersService, IOrdersService ordersService)
        {
            this.workersService = workersService;
            this.ordersService = ordersService;
        }

        [HttpGet("closest")]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> GetClosestWorkers([FromQuery]double latitude, [FromQuery]double longitude, [FromQuery(Name = "category")]int categoryId)
        {
            var clientData = new ClientCallHelpDTO
            {
                Latitude = latitude,
                Longitude = longitude,
                CategoryId = categoryId
            };

            if (!TryValidateModel(clientData) )
            {
                return BadRequest(ModelState);
            }

            if (!ordersService.ValidateOrderCategory(clientData.CategoryId))
            {
                return BadRequest("wrong order's category");
            }

            var workers = await workersService.GetClosestWorkersAsync(clientData);
            if (workers == null)
            {
                return NotFound();
            }

            return Ok(workers);
        }
    }
}