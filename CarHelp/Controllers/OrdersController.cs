﻿using System;
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
    [Route("api/orders")]
    public class OrdersController : Controller
    {
        private readonly IOrdersService ordersService;
        private readonly IWorkersService workersService;
        private readonly IAccountService accountService;

        public OrdersController(IOrdersService ordersService, IWorkersService workersService, IAccountService accountService)
        {
            this.ordersService = ordersService;
            this.workersService = workersService;
            this.accountService = accountService;
        }

        // POST: api/orders
        /// <summary> Create order </summary>
        /// <response code="201"> order has been successfully created </response>
        /// <response code="400"> errors in model valdation or worker unsupported category </response>
        /// <response code="401"> Unathorized </response>
        /// <response code="404"> worker is offline </response>
        /// <response code="500"> erors while storing order in database </response>
        [HttpPost]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> CreateOrder(OrderCreateDTO orderData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var supportedCategory = await workersService.GetSupportedCategoryAsync(orderData.WorkerId, orderData.CategoryId);
            if (supportedCategory == null)
            {
                return BadRequest("worker unsupported category");
            }

            if (!await workersService.WorkerIsOnlineAsync(orderData.WorkerId))
            {
                return NotFound();
            }

            var order = await ordersService.CreateOrderAsync(orderData, User.GetUserId(), supportedCategory);
            if (order == null)
            {
                return StatusCode(500);
            }

            var workerProfile = await accountService.GetUserProfileAsync(order.WorkerId);
            var orderVM = Mapper.Map<CreatedOrderVM>(order);
            orderVM.Worker = Mapper.Map<UserProfileVM>(workerProfile);

            return Ok(orderVM);
        }
    }
}