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
        /// <response code="201"> order has been successfully created </response>
        /// <response code="400"> errors in model valdation or worker unsupported category </response>
        /// <response code="401"> Unathorized </response>
        [HttpPost]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> CreateOrder(CreateOrderInput orderData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = await ordersService.PlaceOrderAsync(orderData, User.GetUserId());
            object orderVM = Mapper.Map<CreatedOrderVM>(order);

            return Ok(orderVM);
        }
    }
}