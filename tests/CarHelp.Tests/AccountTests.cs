using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
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
            _server = new TestServer(new WebHostBuilder()
            .UseStartup<Startup>());

            _client = _server.CreateClient();
        }

        [Fact]
        public async Task SendSmsCodeBadRequestTest()
        {
            var builder = new UriBuilder("/api/auth/sms_code");
            builder.Query = "phone=12313";

            var response = await _client.GetAsync(builder.Uri);

            Assert.Equal(response.StatusCode.ToString(), StatusCodes.Status400BadRequest.ToString());
        }
    }
}
