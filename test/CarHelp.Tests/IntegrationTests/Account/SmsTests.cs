using Microsoft.Extensions.Configuration;
using Respawn;
using Respawn.Postgres;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CarHelp.Tests.IntegrationTests.Account
{
    [Collection("Database collection")]
    public class SmsTests : IClassFixture<CustomWebAppFactory<Startup>>
    {
        private readonly CustomWebAppFactory<Startup> factory;
        private readonly HttpClient client;

        public SmsTests(CustomWebAppFactory<Startup> factory)
        {
            this.factory = factory;

            client = factory.CreateClient();
            client.BaseAddress = new Uri(client.BaseAddress, factory.ApiVersionPrefix);
        }

        [Theory]
        [InlineData("")]
        [InlineData("456465")]
        [InlineData(null)]
        public async Task SMSBadPhoneReturns400(string phone)
        {
            // Arrange
            var uri = $"sms_code?phone={phone}";

            // Act
            var response = await client.GetAsync(uri);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("79256542214")]
        public async Task SMSCorrectPhoneReturns200(string phone)
        {
            // Arrange
            string uri = $"sms_code?phone={phone}";

            // Act
            var response = await client.GetAsync(uri);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
