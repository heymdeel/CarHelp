using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CarHelp.BLL.Model.DTO;
using CarHelp.BLL.Services;
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
        private readonly IWorkersService workersService;
        private readonly IOrdersService ordersService;

        public WorkersController(IWorkersService workersService, IOrdersService ordersService)
        {
            this.workersService = workersService;
            this.ordersService = ordersService;
        }

        // GET: api/workers/closest?latitude&longitude&category
        /// <summary> Get list of workers in user's radius which can perform order </summary>
        /// <response code="200"> list of workers with distances and prices </response>
        /// <response code="400"> errors in model validation or wrong order category</response>
        /// <response code="404"> no workers were found =\ Sorry, mate </response>
        [HttpGet("closest")]
        [Authorize(Roles = "client")]
        [ProducesResponseType(typeof(ClosestWorkersVM), 200)]
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

            var workersVM = Mapper.Map<IEnumerable<ClosestWorkersVM>>(workers);

            return Ok(workersVM);
        }
    }
}