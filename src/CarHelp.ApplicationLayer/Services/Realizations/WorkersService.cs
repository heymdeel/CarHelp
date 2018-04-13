using CarHelp.AppLayer.Models.BusinessModels;
using CarHelp.AppLayer.Models.DTO;
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
    public class WorkersService : IWorkersService
    {
        private readonly IWorkersRepository workersRepository;
        private readonly IRepository<WorkerSupportedCategories> supportedCategoriesRepo;

        public WorkersService(IWorkersRepository workersRepository, IRepository<WorkerSupportedCategories> supportedCategoriesRepo)
        {
            this.workersRepository = workersRepository;
            this.supportedCategoriesRepo = supportedCategoriesRepo;
        }

        public async Task<IEnumerable<ClosestWorkerInfoDTO>> GetClosestWorkersAsync(ClientCallHelpDTO clientData)
        {
            var workers = await workersRepository.GetClosestWorkersAsync(clientData.Longitude, clientData.Latitude, 5000, clientData.CategoryId);

            if (workers?.Count() == 0)
            {
                return null;
            }

            return workers;
        }

        public async Task<WorkerSupportedCategories> GetSupportedCategoryAsync(int workerId, int categoryId)
        {
            return await supportedCategoriesRepo.FirstOrDefaultAsync(c => c.CategoryId == categoryId && c.WorkerId == workerId);
        }

        public async Task<bool> WorkerIsOnlineAsync(int workerId)
        {
            return await workersRepository.FirstOrDefaultAsync(filter: w => w.Id == workerId && w.StatusId == (int)WorkersStatuses.Online) != null;
        }
    }
}
