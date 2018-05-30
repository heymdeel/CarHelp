using CarHelp.DAL.Entities;
using CarHelp.DAL.Models.DTO;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.SqlQuery;
using Microsoft.Extensions.Options;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CarHelp.DAL.Repositories
{
    public class WorkersRepository : Repository<Worker>, IWorkersRepository
    {
        public WorkersRepository(IOptions<ConnectionOptions> options) : base(options) { }

        public async Task<IEnumerable<ClosestWorkerInfoDTO>> GetClosestWorkersAsync(double longitude, double latitude, double radius, int categoryId)
        {
            using (var db = new DataContext(connectionString))
            {
                string sql = @"select workers_price.id, price, name, surname, phone, car_number, 
                                   ST_Distance_Sphere(location, ST_Point(@longitude, @latitude)) as distance from 
	                                    (select id, price, location from 
		                                    (select id, location from workers 
		                                    where ST_DWithin(location::geography, ST_Point(@longitude, @latitude)::geography, @radius) and status = 1) as closest_workers 
	                                    inner join worker_supported_categories on closest_workers.id = id_worker where id_category = @category limit 20) as workers_price
                                   inner join user_profiles on workers_price.id = user_profiles.id
                                   order by distance asc";

                var parameters = new[]
                {
                    new DataParameter("longitude", longitude),
                    new DataParameter("latitude", latitude),
                    new DataParameter("radius", radius),
                    new DataParameter("category", categoryId),
                };

                return await db.QueryToListAsync<ClosestWorkerInfoDTO>(sql, parameters);
            }
        }
    }
}
