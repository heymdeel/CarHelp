using CarHelp.DAL.Entities;
using LinqToDB;
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

        public async Task<IEnumerable<(double price, double distance, UserProfile worker)>> GetClosestWorkersAsync(double longitude, double latitude, double radius, int categoryId)
        {
            var workers = new List<(double price, double distance, UserProfile worker)>();

            using (var conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();

                    string sql = @"select workers_price.id, price, name, surname, phone, car_number, 
                                   ST_Distance_Sphere(location, ST_Point(@longitude, @latitude)) as distance from 
	                                    (select id, price, location from 
		                                    (select id, location from workers 
		                                    where ST_DWithin(location::geography, ST_Point(@longitude, @latitude)::geography, @radius) and status = 1) as closest_workers 
	                                    inner join worker_supported_categories on closest_workers.id = id_worker where id_category = @category limit 20) as workers_price
                                   inner join user_profiles on workers_price.id = user_profiles.id
                                   order by distance asc";


                    
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("latitude", latitude);
                        cmd.Parameters.AddWithValue("longitude", longitude);
                        cmd.Parameters.AddWithValue("radius", radius);
                        cmd.Parameters.AddWithValue("category", categoryId);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                double price = Convert.ToDouble(reader["price"]);
                                double distance = Convert.ToDouble(reader["distance"]);

                                var profile = new UserProfile
                                {
                                    Id = Convert.ToInt32(reader["id"]),
                                    Name = reader["name"].ToString(),
                                    Surname = reader["surname"].ToString(),
                                    Phone = reader["phone"].ToString(),
                                    CarNumber = reader["car_number"].ToString()
                                };

                                workers.Add((price, distance, profile));
                            }
                        }
                    }
                }
                catch (NpgsqlException e)
                {
                    Console.WriteLine(e.ToString());
                    throw;
                }
                finally
                {
                    conn.Close();
                }

                return workers;
            }
        }
    }
}
