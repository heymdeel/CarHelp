using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CarHelp.AppLayer.Models.DTO;
using CarHelp.AppLayer.Services;
using CarHelp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarHelp.Controllers
{
    [Produces("application/json")]
    [Route("api/workers")]
    public class WorkersController : Controller
    {
        private readonly IOrdersService ordersService;

        public WorkersController(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        // GET: api/workers/closest?longitude&latitude&category
        /// <summary> Get list of workers in user's radius who can perform order </summary>
        /// <response code="200"> list of workers with distances and prices </response>
        /// <response code="400"> errors in model validation or wrong order category</response>
        /// <response code="401"> Unathorized </response>
        [HttpGet("closest")]
        [Authorize(Roles = "client")]
        [ProducesResponseType(typeof(ClosestWorkersVM), 200)]
        public async Task<IActionResult> GetClosestWorkers([FromQuery]double longitude, [FromQuery]double latitude, [FromQuery(Name = "category")]int categoryId)
        {
            var clientData = new ClientCallHelpDTO
            {
                Longitude = longitude,
                Latitude = latitude,
                CategoryId = categoryId
            };

            if (!TryValidateModel(clientData))
            {
                return BadRequest(ModelState);
            }

            var workers = await ordersService.FindClosestWorkersAsync(clientData);
            var workersVM = Mapper.Map<IEnumerable<ClosestWorkersVM>>(workers);

            return Ok(workersVM);
        }
    }
}