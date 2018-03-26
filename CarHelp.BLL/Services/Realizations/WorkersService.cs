using CarHelp.BLL.Model.DTO;
using CarHelp.DAL.Entities;
using CarHelp.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarHelp.BLL.Services
{
    public class WorkersService : IWorkersService
    {
        private readonly IWorkersRepository workersRepository;

        public WorkersService(IWorkersRepository workersRepository)
        {
            this.workersRepository = workersRepository;
        }

        public async Task<IEnumerable<(double price, UserProfile worker)>> GetClosestWorkersAsync(ClientCallHelpDTO clientData)
        {
            var workers = await workersRepository.GetClosestWorkersAsync(clientData.Latitude, clientData.Longitude, 5000, clientData.CategoryId);

            if (workers.Count() == 0)
            {
                return null;
            }

            return workers;
        }
    }
}
