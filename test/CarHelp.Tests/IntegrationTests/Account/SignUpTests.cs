using CarHelp.AppLayer.Models.DTO;
using CarHelp.DAL;
using CarHelp.DAL.Entities;
using LinqToDB;
using LinqToDB.Data;
using Newtonsoft.Json;
using Respawn.Postgres;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CarHelp.Tests.IntegrationTests.Account
{
    public class SignUpTestCase : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            // TODO: read this from json?
            yield return new object[] { "", 1111, "Ivan", "Ivanov", "some_model", "x745кк" };
            yield return new object[] { null, 1111, "Ivan", "Ivanov", "some_model", "x745кк" };
            yield return new object[] { "1234", 1111, "Ivan", "Ivanov", "some_model", "x745кк" };
            yield return new object[] { "79293333333", null, "Ivan", "Ivanov", "some_model", "x745кк" };
            yield return new object[] { "79293333333", 12, "Ivan", "Ivanov", "some_model", "x745кк" };
            yield return new object[] { "79293333333", 111111, "Ivan", "Ivanov", "some_model", "x745кк" };
            yield return new object[] { "79293333333", 1111, null, "Ivanov", "some_model", "x745кк" };
            yield return new object[] { "79293333333", 1111, "", "Ivanov", "some_model", "x745кк" };
            yield return new object[] { "79293333333", 1111, "Ivaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaan", "Ivanov", "some_model", "x745кк" };
            yield return new object[] { "79293333333", 1111, "Ivan", null, "some_model", "x745кк" };
            yield return new object[] { "79293333333", 1111, "Ivan", "", "some_model", "x745кк" };
            yield return new object[] { "79293333333", 1111, "Ivan", "Ivanooooooooooooooooooooooooooooooooooooooov", "some_model", "x745кк" };
            yield return new object[] { "79293333333", 1111, "Ivan", "Ivanov", null, "x745кк" };
            yield return new object[] { "79293333333", 1111, "Ivan", "Ivanov", "", "x745кк" };
            yield return new object[] { "79293333333", 1111, "Ivan", "Ivanov", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaasome_model", "x745кк" };
            yield return new object[] { "79293333333", 1111, "Ivan", "Ivanov", "some_model", null };
            yield return new object[] { "79293333333", 1111, "Ivan", "Ivanov", "some_model", "" };
            yield return new object[] { "79293333333", 1111, "Ivan", "Ivanov", "some_model", "x745ккasdasdasdasdasdasdasdasdasdasdads" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }


    [Collection("Database collection")]
    public class SignUpTests : IClassFixture<CustomWebAppFactory<Startup>>, IAsyncLifetime
    {
        private readonly DatabaseFixture fixture;
        private readonly CustomWebAppFactory<Startup> factory;
        private readonly HttpClient client;
        private readonly ITestOutputHelper output;

        public SignUpTests(CustomWebAppFactory<Startup> factory, DatabaseFixture fixture, ITestOutputHelper output)
        {
            this.fixture = fixture;
            this.factory = factory;
            this.output = output;

            client = factory.CreateClient();
            client.BaseAddress = new Uri(client.BaseAddress, factory.ApiVersionPrefix);
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            await DBDeleteuser("79291111111");
            await DBDeleteuser("79292222222");
            await DBDeleteSMSCodeAsync("79291111111");
            await DBDeleteSMSCodeAsync("79292222222");
        }

        [Fact]
        public async Task SignUpReturns200()
        {
            // arrange
            await DBInsertSMSCodeAsync("79291111111", 1111);
            var userProfile = new
            {
                phone = "79291111111",
                sms_code = 1111,
                profile = new
                {
                    name = "Ivan",
                    surname = "Ivanov",
                    car_model = "some_model",
                    car_number = "x111кк"
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(userProfile), Encoding.UTF8, "application/json");

            // act
            var response = await client.PostAsync("sign_up", content);

            // assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Theory]
        [ClassData(typeof(SignUpTestCase))]
        public async Task SignUpBadModelReturns400(string phone, int? code, string name, string surname, string carModel, string carNumber)
        {
            // arrange
            var userProfile = new
            {
                phone = phone,
                sms_code = code,
                profile = new
                {
                    name = name,
                    surname = surname,
                    car_model = carModel,
                    car_number = carNumber
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(userProfile), Encoding.UTF8, "application/json");

            // act
            var response = await client.PostAsync("sign_up", content);

            // assert
            output.WriteLine(await response.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task SignUpNullCodeReturns400()
        {
            // arrange
            var userProfile = new
            {
                phone = "79293333333",
                profile = new
                {
                    name = "Ivan",
                    surname = "Ivanov",
                    car_model = "some_model",
                    car_number = "x745кк"
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(userProfile), Encoding.UTF8, "application/json");

            // act
            var response = await client.PostAsync("sign_up", content);

            // assert
            output.WriteLine(await response.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task SignUpNullProfileReturns400()
        {
            // arrange
            var userProfile = new
            {
                phone = "79293333333",
                sms_code = 1111,
            };
            var content = new StringContent(JsonConvert.SerializeObject(userProfile), Encoding.UTF8, "application/json");

            // act
            var response = await client.PostAsync("sign_up", content);

            // assert
            output.WriteLine(await response.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task SignUpUserExistsReturns400()
        {
            // arrange
            await DBInsertSMSCodeAsync("79292222222", 1111);
            await DBInsertUser("79292222222");
            var userProfile = new
            {
                phone = "79292222222",
                sms_code = 1111,
                profile = new
                {
                    name = "Ivan",
                    surname = "Ivanov",
                    car_model = "some_model",
                    car_number = "x745кк"
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(userProfile), Encoding.UTF8, "application/json");

            // act
            var response = await client.PostAsync("sign_up", content);

            // assert
            output.WriteLine(await response.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task SignUpInvalidCodeReturns400()
        {
            // arrange
            await DBInsertSMSCodeAsync("79294444444", 1111);
            var userProfile = new
            {
                phone = "79292222222",
                sms_code = 2222,
                profile = new
                {
                    name = "Ivan",
                    surname = "Ivanov",
                    car_model = "some_model",
                    car_number = "x444кк"
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(userProfile), Encoding.UTF8, "application/json");

            // act
            var response = await client.PostAsync("sign_up", content);

            // assert
            output.WriteLine(await response.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        private async Task DBInsertUser(string phone)
        {
            var user = new User
            {
                Phone = phone,
                DateOfRegistration = DateTime.Now,
                Roles = new[] { "client", "worker" }
            };

            using (var db = new DbContext(fixture.ConnectionString))
            {
                await db.InsertAsync(user);
            }
        }

        private async Task DBDeleteuser(string phone)
        {
            using (var db = new DbContext(fixture.ConnectionString))
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Phone == phone);

                if (user != null)
                {
                    await db.Workers.DeleteAsync(w => w.Id == user.Id);
                    await db.UserProfiles.DeleteAsync(up => up.Id == user.Id);
                    await db.DeleteAsync(user);
                }
                
            }
        }

        private async Task DBInsertSMSCodeAsync(string phone, int code)
        {
            var smsCode = new SmsCode
            {
                Phone = phone,
                Code = code,
                TimeStamp = DateTime.Now
            };

            using (var db = new DbContext(fixture.ConnectionString))
            {
                await db.InsertAsync(smsCode);
            }
        }

        private async Task DBDeleteSMSCodeAsync(string phone)
        {
            using (var db = new DbContext(fixture.ConnectionString))
            {
                await db.SmsCodes.Where(s => s.Phone == phone).DeleteAsync();
            }
        }
    }
}
