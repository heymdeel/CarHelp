using CarHelp.AppLayer.Models.DTO;
using CarHelp.DAL.Entities;
using CarHelp.DAL.Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarHelp.AppLayer.Services
{
    public interface IWorkersService
    {
        Task<IEnumerable<ClosestWorkerInfoDTO>> GetClosestWorkersAsync(ClientCallHelpDTO clientData);
        Task<WorkerSupportedCategories> GetSupportedCategoryAsync(int workerId, int categoryId);
        Task<bool> WorkerIsOnlineAsync(int workerId);
    }
}
