using Microsoft.Extensions.Configuration;
using Respawn.Postgres;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CarHelp.Tests
{
    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

    public class DatabaseFixture : IAsyncLifetime
    {
        private readonly PostgresCheckpoint checkpoint;

        public string ConnectionString { get; }

        public DatabaseFixture()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\")))
                .AddJsonFile("appsettings_test.json")
                .AddEnvironmentVariables()
                .Build();

            ConnectionString = configuration.GetConnectionString("DefaultConnection");

            checkpoint = new PostgresCheckpoint()
            {
                TablesToIgnore = new[] { "orders_categories", "orders_status", "workers_status"},
                AutoCreateExtensions = true,
                SchemasToInclude = new[] { "public" }
            };
        }

        public async Task DisposeAsync()
        {
            await checkpoint.Reset(ConnectionString);
        }

        public async Task InitializeAsync()
        {
            await checkpoint.Reset(ConnectionString);
        }
    }
}
