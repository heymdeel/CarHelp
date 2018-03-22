using CarHelp.DAL.Entities;
using LinqToDB;
using LinqToDB.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.DAL
{
    internal class ConnectionStringSettings : IConnectionStringSettings
    {
        public string ConnectionString { get; set; }
        public string Name { get; set; }
        public string ProviderName { get; set; }
        public bool IsGlobal => false;
    }

    internal class MySettings : ILinqToDBSettings
    {
        public IEnumerable<IDataProviderSettings> DataProviders
        {
            get { yield break; }
        }

        public string DefaultConfiguration => "Npgsql";
        public string DefaultDataProvider => "Npgsql";

        public IEnumerable<IConnectionStringSettings> ConnectionStrings
        {
            get
            {
                yield return
                    new ConnectionStringSettings
                    {
                        Name = "PostgresConfig",
                        ProviderName = "Npgsql",
                        ConnectionString = "User ID=postgres; Password=359741268; Server=localhost; Port=5432; Database=car_help; Pooling=true;"
                    };
            }
        }
    }

    internal partial class L2DBContext : LinqToDB.Data.DataConnection
    {
        public ITable<SmsCode> SmsCodes { get => GetTable<SmsCode>(); }
        public ITable<Order> Orders { get => GetTable<Order>(); }
        public ITable<OrderCategory> OrdersCategorires { get => GetTable<OrderCategory>(); }
        public ITable<OrderStatus> OrdersStatus { get => GetTable<OrderStatus>(); }
        public ITable<User> Users { get => GetTable<User>(); }
        public ITable<UserProfile> UserProfiles { get => GetTable<UserProfile>(); }
        public ITable<Worker> Workers { get => GetTable<Worker>(); }
        public ITable<WorkerStatus> WorkersStatus { get => GetTable<WorkerStatus>(); }
        public ITable<WorkerSupportedCategories> WorkerSupportedCategories { get => GetTable<WorkerSupportedCategories>(); }

        static L2DBContext()
        {
            DefaultSettings = new MySettings();
            TurnTraceSwitchOn();
            WriteTraceLine = (s1, s2) =>
            {
                Console.WriteLine(s1, s2);
            };
            //LinqToDB.Common.Configuration.Linq.AllowMultipleQuery = true;
        }

        public L2DBContext() : base("PostgresConfig") { }

    }

}
