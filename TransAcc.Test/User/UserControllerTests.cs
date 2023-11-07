using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TransAcc.Test;
using TransAccount.Database;
using TransAccount.User;

namespace TransAccount.Test.User
{
    public class UserControllerTest : IDisposable
    {
        private const string URL = "/api/user";
        private readonly WebApplicationFactory<Program> factory;
        private readonly IServiceScope scope;
        private readonly TransAccContext data;
        private readonly RestClient restClient;
        private bool disposedValue;

        public UserControllerTest()
        {
            this.factory = TestHelpers.UseSqlite(new WebApplicationFactory<Program>(), DatabaseName);
            this.scope = this.factory.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            this.data = FillDatabase(this.scope);
            HttpClient client = this.factory.CreateClient(new WebApplicationFactoryClientOptions());
            this.restClient = new(client);
        }

        public static string DatabaseName => "User";

        protected static TransAccContext FillDatabase(IServiceScope serviceScope)
        {
            using var connection = new Microsoft.Data.Sqlite.SqliteConnection(TestHelpers.GetConnectionString(DatabaseName));
            Microsoft.Data.Sqlite.SqliteConnection.ClearPool(connection);
            var transAccContext = serviceScope.ServiceProvider.GetService<TransAccContext>() ?? throw new NullReferenceException();
            transAccContext.Database.EnsureCreated();
            List<DbUser> users = new()
            {
                new()
                {
                    UserID = 1,
                    FirstName = "Noura",
                    LastName = "Hlayhel",
                    Email = "NouraHlayhel@gmail.com",
                    Password = "Noura123",
                },
                new()
                {
                    UserID = 2,
                    FirstName = "Rawane",
                    LastName = "Masri",
                    Email = "RawaneMasri@gmail.com",
                    Password = "Rawane123",
                },
            };
            transAccContext.Users.AddRange(users);

            transAccContext.SaveChanges();
            return transAccContext;
        }


        [Fact]
        public async Task GetUserWrongPassword_ShouldReturnError()
        {
            var validEmail = "NouraHlayhel@gmail.com";
            var validPassword = "Noura1";
            var url = $"{URL}/authenticate";
            LoginDto loginDto = new()
            {
                Email = validEmail,
                Password = validPassword
            };
            RestRequest? restRequest = new(url)
            {
                Method = Method.Post,
            };
            restRequest.AddBody(loginDto);

            // Act
            RestResponse? response = await this.restClient.ExecuteAsync(restRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        [Fact]
        public async Task GetUserWrongEmail_ShouldReturnError()
        {
            var validEmail = "NouraHlayhel12@gmail.com";
            var validPassword = "Noura123";
            var url = $"{URL}/authenticate";
            LoginDto loginDto = new()
            {
                Email = validEmail,
                Password = validPassword
            };
            RestRequest? restRequest = new(url)
            {
                Method = Method.Post,
            };
            restRequest.AddBody(loginDto);

            // Act
            RestResponse? response = await this.restClient.ExecuteAsync(restRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetUserValidCredentials_ShouldReturnUser()
        {
            var validEmail = "NouraHlayhel@gmail.com";
            var validPassword = "Noura123";
            var url = $"{URL}/authenticate";
            LoginDto loginDto = new()
            {
                Email = validEmail,
                Password = validPassword
            };
            RestRequest? restRequest = new(url)
            {
                Method = Method.Post,
            };
            restRequest.AddBody(loginDto);

            // Act
            RestResponse? response = await this.restClient.ExecuteAsync(restRequest);

            // Assert
            var DbValidUser = await this.data.Users.FirstAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var deserializedResponseContent = JsonSerializer.Deserialize<UserDto>(response.Content ?? string.Empty, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, });
            deserializedResponseContent.Should().BeEquivalentTo(new UserModel(DbValidUser).toDtoModel());
        }
        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.factory.Dispose();
                }

                this.disposedValue = true;
            }
        }
    }
}
