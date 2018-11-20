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
        private readonly IRepository<RespondedWorkers> respondedWorkersRepository;
        private readonly IMapper mapper;

        public OrdersService(IOrdersRepository ordersRepository, IRepository<RespondedWorkers> respondedWorkersRepository, IMapper mapper)
        {
            this.ordersRepository = ordersRepository;
            this.respondedWorkersRepository = respondedWorkersRepository;
            this.mapper = mapper;
        }

        public async Task AttachWorkerToOrderAsync(int clientId, int orderId, AttachWorkerInfo workerInfo)
        {
            if (clientId == workerInfo.Id)
            {
                throw new BadInputException("can not attach yourself to order");
            }

            var order = await ordersRepository.FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null)
            {
                throw new BadInputException("order was not found");
            }

            if (order.ClientId != clientId)
            {
                throw new AccessRefusedException("client has no rights on this order");
            }

            var respondedWorker = await respondedWorkersRepository.FirstOrDefaultAsync(rw => rw.OrderId == orderId && rw.WorkerId == workerInfo.Id);
            if (respondedWorker == null)
            {
                throw new BadInputException("worker was not responding to this order");
            }

            order.WorkerId = workerInfo.Id;
            await ordersRepository.UpdateAsync(order);
        }

        public async Task<IEnumerable<ClosestOrderDTO>> FindClosestOrdersAsync(SearchOrderInput searchInput)
        {
            var searchDTO = mapper.Map<DALSearchOrderDTO>(searchInput);

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

            var order = mapper.Map<Order>(orderInput);
            order.ClientId = clientId;
            order.BeginningTime = DateTime.Now;
            order.StatusId = (int)OrdersStatuses.Awaiting;
            order.Rate = 0;

            order.Location = new Point(new Coordinate(orderInput.Location.Longitude, orderInput.Location.Latitude))
            {
                SRID = 4326
            };

            int orderId = await ordersRepository.InsertWithIdAsync(order);

            return await ordersRepository.FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task RespondToOrderAsync(int orderId, int workerId, WorkerRespondOrderInput workerData)
        {
            var order = await ordersRepository.FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null)
            {
                throw new BadInputException("order with this id was not found");
            }

            if (order.ClientId == workerId)
            {
                throw new BadInputException("can not respond to self placed order");
            }

            if (order.StatusId != (int)OrdersStatuses.Awaiting)
            {
                throw new BadInputException("can't respond to this order, order status is not \"awaiting\"");
            }

            var respondedWorker = mapper.Map<RespondedWorkers>(workerData);
            respondedWorker.OrderId = orderId;
            respondedWorker.WorkerId = workerId;

            await respondedWorkersRepository.InsertAsync(respondedWorker);
        }

        private bool CategoryIsValid(int categoryId)
        {
            return Enum.IsDefined(typeof(OrdersCategories), categoryId);
        }
    }
}
