using AutoMapper;
using CarHelp.AppLayer.Models;
using CarHelp.AppLayer.Models.DTO;
using CarHelp.DAL.DTO;
using CarHelp.DAL.Entities;
using CarHelp.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarHelp.AppLayer.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository ordersRepository;

        public OrdersService(IOrdersRepository ordersRepository)
        {
            this.ordersRepository = ordersRepository;
        }

        public async Task<Order> CreateOrderAsync(OrderCreateDTO orderData, int clientId, WorkerSupportedCategories supportedCategory)
        {
            var orderDTO = Mapper.Map<DALOrderCreateDTO>(orderData);
            orderDTO.BeginingTIme = DateTime.Now;
            orderDTO.StatusId = (int)OrdersStatuses.Awaiting;
            orderDTO.ClientId = clientId;
            orderDTO.Price = supportedCategory.Price;
            orderDTO.Rate = 0;

            return await ordersRepository.InsertOrderAsync(orderDTO);
        }

        public bool ValidateOrderCategory(int categoryId)
        {
            return Enum.IsDefined(typeof(OrdersCategories), categoryId);
        }
    }
}
