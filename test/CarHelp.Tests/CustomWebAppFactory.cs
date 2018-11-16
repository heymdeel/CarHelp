using CarHelp.DAL.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn.Postgres;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace CarHelp.Tests
{
    public class CustomWebAppFactory<TStartup> : WebApplicationFactory<Startup>, IAsyncLifetime
    {
        private readonly IConfiguration configuration;
        private readonly PostgresCheckpoint checkpoint;

        public CustomWebAppFactory()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\")))
                .AddJsonFile("appsettings_test.json")
                .AddEnvironmentVariables()
                .Build();
            
            checkpoint = new PostgresCheckpoint()
            {
                TablesToIgnore = new[] { "orders_categories", "orders_status", "workers_status" },
                AutoCreateExtensions = true,
                SchemasToInclude = new [] { "public" }
            };
        }

        public async Task DisposeAsync() { }

        public async Task InitializeAsync()
        {
            await checkpoint.Reset(configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task ResetDB() => await checkpoint.Reset(configuration.GetConnectionString("DefaultConnection"));

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddOptions();
                services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));
            });            
        }
    }
}
