using AutoMapper;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CarHelp.Tests.Account
{
    public class SmsTests
    {
        private readonly HttpClient _client;

        public SmsTests()
        {
            var server = new WebApplicationFactory<Startup>();

            Mapper.Reset();

            _client = server.CreateClient();
            
        }

        [Theory]
        [InlineData("")]
        public async Task EmptyPhoneReturns400(string phone)
        {
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
            // Arrange
            var uri = $"api/sms_code?phone={phone}";

            // Act
            var response = await _client.GetAsync(uri);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
