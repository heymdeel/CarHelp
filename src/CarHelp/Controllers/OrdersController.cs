using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CarHelp.AppLayer.Models.DTO;
using CarHelp.AppLayer.Services;
using CarHelp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarHelp.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/orders")]
    [Route("api/orders")]
    [Produces("application/json")]
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

        // GET: api/orders/closest?longitude&latitude&categories&pricefrom&priceto&distance&limit
        /// <summary> Find closest orders by worker </summary>
        /// <response code="200"> list of orders </response>
        /// <response code="400"> errors in model valdation </response>
        /// <response code="401"> Unathorized </response>
        [HttpGet("closest")]
        [Authorize(Roles = "worker")]
        [ProducesResponseType(typeof(ClosestOrderVM), 200)]
        public async Task<IActionResult> GetListOfOrders([FromQuery]SearchOrderInput searchInput)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orders = await ordersService.FindClosestOrdersAsync(searchInput);
            var ordersVM = Mapper.Map<IEnumerable<ClosestOrderVM>>(orders);

            return Ok(ordersVM);
        }

        // PUT: api/orders/{id}/responded_workers
        /// <summary> Respond to an order </summary>
        /// <response code="200"> ok </response>
        /// <response code="400"> bad order status, errors in model validation or order was not found </response>
        /// <response code="401"> unauthorized </response>
        [HttpPut("{id:int}/responded_workers")]
        [Authorize(Roles = "worker")]
        public async Task<IActionResult> RespondToOrder([FromRoute(Name = "id")]int orderId, [FromBody]WorkerRespondOrderInput workerData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int workerId = User.GetUserId();
            await ordersService.RespondToOrderAsync(orderId, workerId, workerData);

            return Ok();
        }

        // PUT: api/orders/{id}/worker
        /// <summary> Attach worker to an order </summary>
        /// <response code="200"> worker was succesfully attached to order </response>
        /// <response code="400"> errors in model validation, absence of order with such id or worker wasn't responding to order </response>
        /// <response code="401"> unauthorized </response>
        /// <response code="403"> client has no rights on updating order info </response>
        [HttpPut("{id:int}/worker")]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> AttachWorkerToOrder([FromRoute(Name = "id")] int orderId, [FromBody] AttachWorkerInfo workerData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            int clientId = User.GetUserId();
            await ordersService.AttachWorkerToOrderAsync(clientId, orderId, workerData);

            return Ok();
        }
    }
}