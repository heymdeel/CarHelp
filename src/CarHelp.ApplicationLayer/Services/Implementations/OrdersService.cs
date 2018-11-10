using AutoMapper;
using CarHelp.AppLayer.Models;
using CarHelp.AppLayer.Models.DTO;
using CarHelp.DAL.DTO;
using CarHelp.DAL.Entities;
using CarHelp.DAL.Repositories;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<ClosestOrderDTO>> FindClosestOrdersAsync(SearchOrderInput searchInput)
        {
            var searchDTO = Mapper.Map<DALSearchOrderDTO>(searchInput);

            return await ordersRepository.FindClosestOrdersAsync(searchDTO);
        }

        public async Task<Order> PlaceOrderAsync(CreateOrderInput orderInput, int clientId)
        {
            if (!CategoryIsValid(orderInput.CategoryId))
            {
                throw new BadInputException("category is not supported");
            }

            if (await ordersRepository.FirstOrDefaultAsync(o => o.ClientId == clientId && o.StatusId < 3) != null)
            {
                throw new BadInputException("client already has an actual order");
            }

            var order = Mapper.Map<Order>(orderInput);
            order.ClientId = clientId;
            order.BeginningTime = DateTime.Now;
            order.StatusId = (int)OrdersStatuses.Awaiting;
            order.Rate = 0;

            order.Location = new Point(new Coordinate(orderInput.Longitude, orderInput.Latitude))
            {
                SRID = 4326
            };

            int orderId = await ordersRepository.InsertWithIdAsync(order);

            return await ordersRepository.FirstOrDefaultAsync(o => o.Id == orderId);
        }

        private bool CategoryIsValid(int categoryId)
        {
            return Enum.IsDefined(typeof(OrdersCategories), categoryId);
        }
    }
}
