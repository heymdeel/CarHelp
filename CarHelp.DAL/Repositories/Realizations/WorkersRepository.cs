using CarHelp.DAL.Entities;
using LinqToDB;
using LinqToDB.SqlQuery;
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
    public class WorkersRepository : L2DBRepository<Worker>, IWorkersRepository
    {
        public async Task<IEnumerable<(double price, double distance, UserProfile worker)>> GetClosestWorkersAsync(double latitude, double longitude, double radius, int categoryId)
        {
            var workers = new List<(double price, double distance, UserProfile worker)>();
            // TODO: move this to configuration or another shared place
            const string connectionString = "User ID=postgres; Password=359741268; Server=localhost; Port=5432; Database=car_help; Pooling=true;";

            using (var conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();

                    // TODO: add pagination?
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

        // TODO: remove this
        public async Task Test()
        {
            var basePhoneNumber = 7000000000;

            string baseName = "user";
            var rand = new Random();

            string[] roles = { "client", "worker" };

            var user = new User();
            var profile = new UserProfile();
            var worker = new Worker();
            var supp = new WorkerSupportedCategories();

            using (var db = new L2DBContext())
            {
                int j = 1;
                for (var i = basePhoneNumber; j < 100000; i++, j++)
                {
                    user.Phone = (basePhoneNumber + j).ToString();
                    user.Roles = roles;
                    user.DateOfRegistration = DateTime.Now;
                    user.Id = j;

                    await db.GetTable<User>()
                            .Value(u => u.Phone, user.Phone)
                            .Value(u => u.Roles, user.Roles)
                            .Value(u => u.DateOfRegistration, user.DateOfRegistration)
                            .Value(u => u.Id, user.Id)
                            .InsertAsync();

                    var kekName = baseName + j.ToString();

                    profile.Name = kekName;
                    profile.Surname = kekName;
                    profile.Phone = user.Phone;
                    profile.CarNumber = kekName;
                    profile.Id = j;

                    await db.InsertAsync(profile);

                    worker.Id = j;
                    worker.StatusId = rand.Next(0, 2);
                    double latitude = 47.2 + rand.NextDouble() % 0.1;
                    double longitude = 39.7 + rand.NextDouble() % 0.1;

                    worker.Location = new PostgisPoint(longitude, latitude);
                    worker.Location.SRID = 4326;

                    await db.InsertAsync(worker);

                    supp.WorkerId = j;
                    supp.CategoryId = rand.Next(0, 3);
                    supp.Price = rand.Next(50, 250);

                    await db.InsertAsync(supp);
                }
            }
        }
    }
}
