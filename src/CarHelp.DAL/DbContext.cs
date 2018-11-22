using CarHelp.DAL.Entities;
using LinqToDB;
using LinqToDB.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using LinqToDB.Data;
using LinqToDB.DataProvider.PostgreSQL;
using Npgsql;

namespace CarHelp.DAL
{
    public partial class DbContext : DataConnection
    {
        public ITable<SmsCode> SmsCodes { get => GetTable<SmsCode>(); }
        public ITable<Order> Orders { get => GetTable<Order>(); }
        public ITable<OrderCategory> OrdersCategorires { get => GetTable<OrderCategory>(); }
        public ITable<OrderStatus> OrdersStatus { get => GetTable<OrderStatus>(); }
        public ITable<User> Users { get => GetTable<User>(); }
        public ITable<UserProfile> UserProfiles { get => GetTable<UserProfile>(); }
        public ITable<Worker> Workers { get => GetTable<Worker>(); }
        public ITable<WorkerStatus> WorkersStatus { get => GetTable<WorkerStatus>(); }
        public ITable<RespondedWorkers> RespondedWorkers { get => GetTable<RespondedWorkers>(); }

        static DbContext()
        {
            NpgsqlConnection.GlobalTypeMapper.UseNetTopologySuite();

            // change this to the normal logging
            TurnTraceSwitchOn();
            WriteTraceLine = (s1, s2) =>
            {
                Console.WriteLine(s1, s2);
            };
        }

        public DbContext(string connectionString): base(new PostgreSQLDataProvider(), connectionString) {}
    }

}
