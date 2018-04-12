using AutoMapper;
using CarHelp.DAL.DTO;
using CarHelp.DAL.Entities;
using LinqToDB;
using Microsoft.Extensions.Options;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarHelp.DAL.Repositories
{
    public class OrdersRepository : Repository<Order>, IOrdersRepository
    {
        public OrdersRepository(IOptions<ConnectionOptions> options) : base(options) { }
     
        public async Task<Order> CreateOrderAsync(DALOrderCreateDTO orderData)
        {
            var order = Mapper.Map<Order>(orderData);
            order.Location = new PostgisPoint(orderData.Longitude, orderData.Latitude)
            {
                SRID = 4326
            };

            using (var db = new DataContext(connectionString))
            {
                int orderId = await db.InsertWithInt32IdentityAsync(order);

                return await db.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
            }
        }
    }
}
