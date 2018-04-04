using CarHelp.BLL.Model.BusinessModels;
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
        private readonly IRepository<WorkerSupportedCategories> supportedCategoriesRepo;

        public WorkersService(IWorkersRepository workersRepository, IRepository<WorkerSupportedCategories> supportedCategoriesRepo)
        {
            this.workersRepository = workersRepository;
            this.supportedCategoriesRepo = supportedCategoriesRepo;
        }

        public async Task<IEnumerable<(double price, double distance, UserProfile worker)>> GetClosestWorkersAsync(ClientCallHelpDTO clientData)
        {
            var workers = await workersRepository.GetClosestWorkersAsync(clientData.Latitude, clientData.Longitude, 5000, clientData.CategoryId);

            if (workers.Count() == 0)
            {
                return null;
            }

            return workers;
        }

        public async Task Test()
        {
            await workersRepository.Test();
        }

        public async Task<WorkerSupportedCategories> GetSupportedCategoryAsync(int workerId, int categoryId)
        {
            return await supportedCategoriesRepo.FirstOrDefaultAsync(c => c.IdCategory == categoryId && c.IdWorker == workerId);
        }

        public async Task<bool> WorkerIsOnlineAsync(int workerId)
        {
            return await workersRepository.FirstOrDefaultAsync(filter: w => w.Id == workerId && w.StatusId == (int)WorkersStatuses.Online) != null;
        }
    }
}
