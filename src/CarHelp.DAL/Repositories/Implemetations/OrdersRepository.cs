using AutoMapper;
using CarHelp.DAL.DTO;
using CarHelp.DAL.Entities;
using GeoAPI.Geometries;
using LinqToDB;
using LinqToDB.Data;
using Microsoft.Extensions.Options;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarHelp.DAL.Repositories
{
    public class OrdersRepository : Repository<Order>, IOrdersRepository
    {
        public OrdersRepository(IOptions<ConnectionStrings> options) : base(options) { }

        public async Task<IEnumerable<ClosestOrderDTO>> FindClosestOrdersAsync(DALSearchOrderDTO searchInput)
        {
            var workerLocation = new Point(searchInput.Longitude, searchInput.Latitude)
            {
                SRID = 4326
            };

            using (var db = new DbContext(connectionString))
            {
                var query = db.Orders.AsQueryable();

                // distance filter
                query = query.Where(o => o.Location.PgisWithinDistance(workerLocation, searchInput.Distance));

                // price filter
                if (searchInput.PriceFrom.HasValue && searchInput.PriceTo.HasValue)
                {
                    query = query.Where(o => o.ClientPrice.Between(searchInput.PriceFrom.Value, searchInput.PriceTo.Value));
                }
                else
                {
                    if (searchInput.PriceFrom.HasValue)
                    {
                        query = query.Where(o => o.ClientPrice >= searchInput.PriceFrom.Value);
                    }
                    else if (searchInput.PriceTo.HasValue)
                    {
                        query = query.Where(o => o.ClientPrice <= searchInput.PriceTo.Value);
                    }
                }

                // categories filter
                if (searchInput.Categories?.Length != 0)
                {
                    query = query.Where(o => searchInput.Categories.Contains(o.CategoryId));
                }

                // limit filter
                if (!searchInput.Limit.HasValue)
                {
                    searchInput.Limit = 40;
                }
                query = query.Take(searchInput.Limit.Value);

                // join
                var joinQuery = from o in query
                            join c in db.UserProfiles on o.ClientId equals c.Id
                            select new { Order = o, Client = c};

                // select
                var selectQuery = joinQuery.Select(result => new ClosestOrderDTO()
                {
                    Id = result.Order.Id,
                    BeginningTime = result.Order.BeginningTime,
                    Category = result.Order.CategoryId,
                    ClientPrice = result.Order.ClientPrice,
                    Commentary = result.Order.Commentary,
                    Location = new OrderLocationDTO
                    {
                        Longitude = result.Order.Location.PgisLongitude(),
                        Latitude = result.Order.Location.PgisLatitude(),
                        Distance = result.Order.Location.PgisDistance(workerLocation)
                    },
                    Client = new ClientOrderDTO()
                    {
                        Id = result.Client.Id,
                        Name = result.Client.Name,
                        Phone = result.Client.Phone,
                        CarNumber = result.Client.CarNumber,
                        CarModel = result.Client.CarModel
                    }
                });

                return await selectQuery.ToListAsync();
            }
        }
    }
}
