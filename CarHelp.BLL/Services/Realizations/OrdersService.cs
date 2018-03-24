using CarHelp.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarHelp.BLL.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IWorkersRepository workersRepository;

        public OrdersService(IWorkersRepository workersRepository)
        {
            this.workersRepository = workersRepository;
        }
    }
}
