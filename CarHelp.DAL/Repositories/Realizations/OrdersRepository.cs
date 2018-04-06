using AutoMapper;
using CarHelp.DAL.DTO;
using CarHelp.DAL.Entities;
using LinqToDB;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarHelp.DAL.Repositories
{
    public class OrdersRepository : L2DBRepository<Order>, IOrdersRepository
    {
        public async Task<Order> CreateOrderAsync(DALOrderCreateDTO orderData)
        {
            var order = Mapper.Map<Order>(orderData);
            order.Location = new PostgisPoint(orderData.Longitude, orderData.Latitude);
            order.Location.SRID = 4326;

            using (var db = new L2DBContext())
            {
                var orderId = (int)(long)await db.InsertWithIdentityAsync(order);

                return await db.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
            }
        }
    }
}
