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
using TransAccount.Account;
using TransAccount.Database;
using TransAccount.Transactions;
using TransAccount.TransactionType;
using TransAccount.User;

namespace TransAccount.Test.Account
{
    /// <summary>
    /// The account controller tests.
    /// </summary>
    public class AccountControllerTests : IDisposable
    {
        /// <summary>
        /// The u r l.
        /// </summary>
        private const string URL = "/api/account";
        private readonly WebApplicationFactory<Program> factory;
        private readonly IServiceScope scope;
        private readonly TransAccContext data;
        private readonly RestClient restClient;
        private bool disposedValue;

        public AccountControllerTests()
        {
            this.factory = TestHelpers.UseSqlite(new WebApplicationFactory<Program>(), DatabaseName);
            this.scope = this.factory.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            this.data = FillDatabase(this.scope);
            HttpClient client = this.factory.CreateClient(new WebApplicationFactoryClientOptions());
            this.restClient = new(client);
        }

        public static string DatabaseName => "Account";

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
            List<DbAccount> accounts = new()
            {
                new()
                {
                    AccountID = 1,
                    CreationDate = DateTime.Now,
                    Balance = 200,
                    CurrencyId = 1,
                    CustomerId = 1,
                }
            };
            List<DbCurrency> currencies = new()
            {
                new()
                {
                    CurrencyId = 1,
                    Code = "USD",
                    Symbol = '$',
                    Name = "United States Dollar"
                }
            };
            List<DbTransactionType> types = new()
            {
                new()
                {
                    TransactionTypeID = 1,
                    Name = Database.TransactionType.Deposit,
                },
                new()
                {
                    TransactionTypeID = 2,
                    Name = Database.TransactionType.Withdrawal,
                }
            };
            transAccContext.Users.AddRange(users);
            transAccContext.Currencies.AddRange(currencies);
            transAccContext.Accounts.AddRange(accounts);
            transAccContext.TransactionTypes.AddRange(types);
            transAccContext.SaveChanges();
            return transAccContext;
        }


        [Fact]
        public async Task CreateNewAccountNoInitialCredit_ReturnsNewAccount()
        {
            var url = $"{URL}";
            CreateAccountDto createAccountDto = new()
            {
                InitialCredit = 0,
                CustomerId = 2,
                CurrencyId = 1
            };
            RestRequest? restRequest = new(url)
            {
                Method = Method.Post,
            };
            restRequest.AddBody(createAccountDto);

            // Act
            RestResponse? response = await this.restClient.ExecuteAsync(restRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var newAccount = await this.data.Accounts.Where(acc => acc.CustomerId == 2).SingleAsync();
            var deserializedResponseContent = JsonSerializer.Deserialize<AccountDto>(response.Content ?? string.Empty, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, });
            deserializedResponseContent.Should().BeEquivalentTo(new TransAccount.Account.Account(newAccount).ToDtoModel());
        }

        [Fact]
        public async Task CreateNewAccountWithInitialCredit_ReturnsNewAccount()
        {
            var url = $"{URL}";
            CreateAccountDto createAccountDto = new()
            {
                InitialCredit = 100,
                CustomerId = 2,
                CurrencyId = 1
            };
            RestRequest? restRequest = new(url)
            {
                Method = Method.Post,
            };
            restRequest.AddBody(createAccountDto);

            // Act
            RestResponse? response = await this.restClient.ExecuteAsync(restRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var newAccount = await this.data.Accounts.Where(acc => acc.CustomerId == 2).SingleAsync();
            var addedTransaction = await this.data.Transactions.Where(t => t.AccountID == newAccount.AccountID).SingleAsync();
            addedTransaction.Amount.Should().Be(100);
            addedTransaction.TypeID.Should().Be(1);
            var expectedAccount = new TransAccount.Account.Account(newAccount).ToDtoModel();
            var deserializedResponseContent = JsonSerializer.Deserialize<AccountDto>(response.Content ?? string.Empty, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, });

            deserializedResponseContent.Should().BeEquivalentTo(expectedAccount, opt => opt.Excluding(a => a.Transactions));
        }
        
        [Fact]
        public async Task CreateNewAccountInvalidUser_ReturnsFalse()
        {
            var url = $"{URL}";
            CreateAccountDto createAccountDto = new()
            {
                InitialCredit = 100,
                CustomerId = 3,
                CurrencyId = 1
            };
            RestRequest? restRequest = new(url)
            {
                Method = Method.Post,
            };
            restRequest.AddBody(createAccountDto);

            // Act
            RestResponse? response = await this.restClient.ExecuteAsync(restRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }


        [Fact]
        public async Task GetAccountsForUser_ReturnsAccounts()
        {
            int validUserID = 1;
            var url = $"{URL}/{validUserID}";
            RestRequest? restRequest = new(url)
            {
                Method = Method.Get,
            };

            // Act
            RestResponse? response = await this.restClient.ExecuteAsync(restRequest);

            // Assert
            var expectedAccounts = await this.data.Accounts.Where(acc => acc.CustomerId == validUserID).ToListAsync();
            var user = await this.data.Users.Where(u => u.UserID == validUserID).SingleAsync();
            var expectedResponse = expectedAccounts.Select(acc =>
                new AccountDto()
                {
                    AccountId = acc.AccountID,
                    Transactions = new List<TransactionDto>() { },
                    User = new UserModel(acc.User).toDtoModel(),
                    CurrencyId = acc.CurrencyId,
                    Balance = acc.Balance,
                }).ToList();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var deserializedResponseContent = JsonSerializer.Deserialize<List<AccountDto>>(response.Content ?? string.Empty, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, });
            deserializedResponseContent.Should().BeEquivalentTo(expectedResponse);

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
