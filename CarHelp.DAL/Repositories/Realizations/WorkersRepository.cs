using CarHelp.DAL.Entities;
using LinqToDB;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarHelp.DAL.Repositories
{
    public class WorkersRepository : L2DBRepository<Worker>, IWorkersRepository
    {
        public async Task<IEnumerable<Worker>> GetClosestWorkers() => throw new NotImplementedException();
    }
}
