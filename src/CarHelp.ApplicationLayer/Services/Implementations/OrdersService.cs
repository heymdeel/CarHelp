using AutoMapper;
using CarHelp.AppLayer.Models;
using CarHelp.AppLayer.Models.DTO;
using CarHelp.DAL.DTO;
using CarHelp.DAL.Entities;
using CarHelp.DAL.Models.DTO;
using CarHelp.DAL.Repositories;
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
        private readonly IWorkersRepository workersRepository;
        private readonly IRepository<WorkerSupportedCategories> categoriesRepo;

        public OrdersService(IOrdersRepository ordersRepository, IWorkersRepository workersRepository, IRepository<WorkerSupportedCategories> categoriesRepo)
        {
            this.ordersRepository = ordersRepository;
            this.workersRepository = workersRepository;
            this.categoriesRepo = categoriesRepo;
        }

        public async Task<IEnumerable<ClosestWorkerkDTO>> FindClosestWorkersAsync(ClientCallHelpDTO clientData)
        {
            if (!CategoryIsValid(clientData.CategoryId))
            {
                throw new BadInputException("wrong order's category");
            }

            return await workersRepository.GetClosestWorkersAsync(clientData.Longitude, clientData.Latitude, 5000, clientData.CategoryId);
        }

        public async Task<Order> PlaceOrderAsync(CreateOrderInput orderInput, int clientId)
        {
            var supportedCategory = await categoriesRepo.FirstOrDefaultAsync(c => c.WorkerId == orderInput.WorkerId && c.CategoryId == orderInput.CategoryId);       
            if (supportedCategory == null)
            {
                throw new BadInputException("worker doesn't support this category");
            }

            if (await workersRepository.FirstOrDefaultAsync(w => w.StatusId == (int)WorkersStatuses.Online) == null)
            {
                throw new BadInputException("worker is offline");
            }

            var orderDTO = CreateOrder(orderInput, clientId, supportedCategory);

            return await ordersRepository.InsertOrderAsync(orderDTO);
        }

        private bool CategoryIsValid(int categoryId)
        {
            return Enum.IsDefined(typeof(OrdersCategories), categoryId);
        }

        private DALOrderCreateDTO CreateOrder(CreateOrderInput orderData, int clientId, WorkerSupportedCategories category)
        {
            var orderDTO = Mapper.Map<DALOrderCreateDTO>(orderData);
            orderDTO.BeginingTIme = DateTime.Now;
            orderDTO.StatusId = (int)OrdersStatuses.Awaiting;
            orderDTO.ClientId = clientId;
            orderDTO.Price = category.Price;
            orderDTO.Rate = 0;

            return orderDTO;
        }
    }
}
