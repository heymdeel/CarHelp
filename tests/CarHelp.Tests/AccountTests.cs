using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CarHelp.Tests
{
    public class AccountTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public AccountTests()
        {
            var projectPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\..\", "src/CarHelp"));

            var config = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            _server = new TestServer(WebHost.CreateDefaultBuilder()
                .UseConfiguration(config)
                .UseStartup<Startup>()
                .UseEnvironment("Development"));

            _client = _server.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:8080/");
        }

        [Theory]
        [InlineData("12345")]
        public async Task SendSmsCodeBadRequestTest(string phone)
        {
            // Arrange
            var uri = $"api/sms_code?phone={phone}";

            // Act
            var response = await _client.GetAsync(uri);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
