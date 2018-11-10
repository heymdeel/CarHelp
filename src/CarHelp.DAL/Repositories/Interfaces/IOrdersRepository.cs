using CarHelp.DAL.DTO;
using CarHelp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarHelp.DAL.Repositories
{
    public interface IOrdersRepository : IRepository<Order>
    {
        Task<IEnumerable<ClosestOrderDTO>> FindClosestOrdersAsync(DALSearchOrderDTO searchInput);
    }
}
