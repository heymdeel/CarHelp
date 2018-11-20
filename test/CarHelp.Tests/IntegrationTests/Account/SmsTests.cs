using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CarHelp.Tests.IntegrationTests.Account
{
    public class SmsTests : IClassFixture<CustomWebAppFactory<Startup>>
    {
        private readonly CustomWebAppFactory<Startup> factory;
        private readonly HttpClient _client;

        public SmsTests(CustomWebAppFactory<Startup> factory)
        {
            this.factory = factory;

            _client = factory.CreateClient();
            _client.BaseAddress = new Uri(_client.BaseAddress, factory.ApiVersionPrefix);
        }

        [Theory]
        [InlineData("")]
        public async Task EmptyPhoneReturns400(string phone)
        {
            // Arrange
            var uri = $"sms_code?phone={phone}";

            // Act
            var response = await _client.GetAsync(uri);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("456465")]
        public async Task WrongPhoneReturns400(string phone)
        {
            // Arrange
            var uri = $"sms_code?phone={phone}";

            // Act
            var response = await _client.GetAsync(uri);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("79256542214")]
        public async Task CorrectPhoneReturns200(string phone)
        {
            // Arrange
            var uri = $"sms_code?phone={phone}";

            // Act
            var response = await _client.GetAsync(uri);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task NullPhoneReturns400()
        {
            // Arrange
            var uri = $"sms_code";

            // Act
            var response = await _client.GetAsync(uri);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
