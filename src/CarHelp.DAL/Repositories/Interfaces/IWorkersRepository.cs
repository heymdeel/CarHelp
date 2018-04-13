using CarHelp.DAL.Entities;
using CarHelp.DAL.Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarHelp.DAL.Repositories
{
    public interface IWorkersRepository : IRepository<Worker>
    {
        Task<IEnumerable<ClosestWorkerInfoDTO>> GetClosestWorkersAsync(double longitude, double latitude, double radius, int categoryId);
    }
}
