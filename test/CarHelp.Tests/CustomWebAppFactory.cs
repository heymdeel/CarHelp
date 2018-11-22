using CarHelp.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Respawn.Postgres;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace CarHelp.Tests
{
    public class CustomWebAppFactory<TStartup> : WebApplicationFactory<Startup>
    {
        private readonly IConfiguration configuration;

        public string ApiVersionPrefix { get; } = "api/v1/";
        
        public CustomWebAppFactory()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\")))
                .AddJsonFile("appsettings_test.json")
                .AddEnvironmentVariables()
                .Build();
        }

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
