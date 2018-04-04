using CarHelp.BLL.Model.DTO;
using CarHelp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarHelp.BLL.Services
{
    public interface IWorkersService
    {
        Task<IEnumerable<(double price, double distance, UserProfile worker)>> GetClosestWorkersAsync(ClientCallHelpDTO clientData);
        Task<WorkerSupportedCategories> GetSupportedCategoryAsync(int workerId, int categoryId);
        Task<bool> WorkerIsOnlineAsync(int workerId);

        Task Test();
    }
}
