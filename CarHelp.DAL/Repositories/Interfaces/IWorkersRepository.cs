using CarHelp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarHelp.DAL.Repositories
{
    public interface IWorkersRepository : IRepository<Worker>
    {
        Task<IEnumerable<(double price, UserProfile worker)>> GetClosestWorkersAsync(double latitude, double longitude, double distance, int categoryId);
    }
}
