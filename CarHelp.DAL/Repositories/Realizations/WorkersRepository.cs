﻿using CarHelp.DAL.Entities;
using LinqToDB;
using LinqToDB.SqlQuery;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarHelp.DAL.Repositories
{
    public class WorkersRepository : L2DBRepository<Worker>, IWorkersRepository
    {
        public async Task<IEnumerable<(double price, UserProfile worker)>> GetClosestWorkersAsync(double latitude, double longitude, double distance, int categoryId)
        {
            var workers = new List<(double price, UserProfile worker)>();
            // TODO: move this to configuration or another shared place
            const string connectionString = "User ID=postgres; Password=359741268; Server=localhost; Port=5432; Database=car_help; Pooling=true;";

            using (var conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();

                    string sql = @"select workers_price.id, price, name, surname, phone, car_number from 
	                                    (select id, price from 
		                                    (select id from workers 
		                                    where ST_Distance_Sphere(location, ST_SetSRID(ST_Point(@latitude, @longitude), 4326)) <= @distance and status = 1) as closest_workers 
	                                    inner join worker_supported_categories on closest_workers.id = id_worker where id_category = @category) as workers_price
                                    inner join user_profiles on workers_price.id = user_profiles.id";

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("latitude", latitude);
                        cmd.Parameters.AddWithValue("longitude", longitude);
                        cmd.Parameters.AddWithValue("distance", distance);
                        cmd.Parameters.AddWithValue("category", categoryId);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                double price = Convert.ToDouble(reader["price"]);
                                var profile = new UserProfile
                                {
                                    Id = Convert.ToInt32(reader["id"]),
                                    Name = reader["name"].ToString(),
                                    Surname = reader["surname"].ToString(),
                                    Phone = reader["phone"].ToString(),
                                    CarNumber = reader["car_number"].ToString()
                                };

                                workers.Add((price, profile));
                            }
                        }
                    }
                }
                catch (Exception e)
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
