using AutoMapper;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CarHelp.Tests.IntegrationTests.Account
{
    public class SmsTests : IClassFixture<CustomWebAppFactory<Startup>>
    {
        private readonly CustomWebAppFactory<Startup> factory;
        private readonly HttpClient _client;
        private readonly ITestOutputHelper output;

        public SmsTests(CustomWebAppFactory<Startup> factory, ITestOutputHelper output)
        {
            this.output = output;
            this.factory = factory;

            Mapper.Reset();

            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData("")]
        public async Task EmptyPhoneReturns400(string phone)
        {
            output.WriteLine(CustomWebAppFactory<Startup>.x.ToString());
            // Arrange
            var uri = $"api/sms_code?phone={phone}";

            // Act
            var response = await _client.GetAsync(uri);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("456465")]
        public async Task WrongPhoneReturns400(string phone)
        {
            output.WriteLine(CustomWebAppFactory<Startup>.x.ToString());
            // Arrange
            var uri = $"api/sms_code?phone={phone}";

            // Act
            var response = await _client.GetAsync(uri);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("79256542214")]
        public async Task CorrectPhoneReturns200(string phone)
        {
            output.WriteLine(CustomWebAppFactory<Startup>.x.ToString());
            // Arrange
            var uri = $"api/sms_code?phone={phone}";

            // Act
            var response = await _client.GetAsync(uri);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PhoneIsNullReturns400()
        {
            output.WriteLine(CustomWebAppFactory<Startup>.x.ToString());
            // Arrange
            var uri = $"api/sms_code";

            // Act
            var response = await _client.GetAsync(uri);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
