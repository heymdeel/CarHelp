using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CarHelp.AppLayer.Models.DTO;
using CarHelp.AppLayer.Services;
using CarHelp.DAL.DTO;
using CarHelp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarHelp.Controllers
{
    [Produces("application/json")]
    [Route("api/orders")]
    public class OrdersController : Controller
    {
        private readonly IOrdersService ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        // POST: api/orders
        /// <summary> Create order </summary>
        /// <response code="200"> order has been successfully created </response>
        /// <response code="400"> errors in model valdation or category is not supported</response>
        /// <response code="401"> Unathorized </response>
        [HttpPost]
        [Authorize(Roles = "client")]
        [ProducesResponseType(typeof(CreatedOrderVM), 200)]
        public async Task<IActionResult> CreateOrder([FromBody]CreateOrderInput orderData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = await ordersService.PlaceOrderAsync(orderData, User.GetUserId());
            var orderVM = Mapper.Map<CreatedOrderVM>(order);

            return Ok(orderVM);
        }

        // GET: api/orders/active?longitude&latitude&categories&pricefrom&priceto&distance&limit
        /// <summary> Find closest orders </summary>
        /// <response code="200"> list of orders </response>
        /// <response code="400"> errors in model valdation </response>
        /// <response code="401"> Unathorized </response>
        [HttpGet("active")]
        [Authorize(Roles = "worker")]
        [ProducesResponseType(typeof(ClosestOrderDTO), 200)]
        public async Task<IActionResult> GetListOfOrders([FromQuery]SearchOrderInput searchInput)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await ordersService.FindClosestOrdersAsync(searchInput);
            
            return Ok(result);
        }
    }
}