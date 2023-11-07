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
using TransAccount.Transactions;
using TransAccount.TransactionType;
using TransAccount.User;

namespace TransAccount.Test.Transaction
{
    public class TransactionsControllerTests : IDisposable
    {
        private const string URL = "/api/transactions";
        private readonly WebApplicationFactory<Program> factory;
        private readonly IServiceScope scope;
        private readonly TransAccContext data;
        private readonly RestClient restClient;
        private bool disposedValue;

        public TransactionsControllerTests()
        {
            this.factory = TestHelpers.UseSqlite(new WebApplicationFactory<Program>(), DatabaseName);
            this.scope = this.factory.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            this.data = FillDatabase(this.scope);
            HttpClient client = this.factory.CreateClient(new WebApplicationFactoryClientOptions());
            this.restClient = new(client);
        }

        public static string DatabaseName => "Transactions";

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
        public async Task AddDepositTransactionValidAccount_ReturnsTrue()
        {
            var url = $"{URL}";
            var account = await this.data.Accounts.FirstAsync();
            TransactionDto transactionDto = new()
            {
                TransactionType = Database.TransactionType.Deposit.ToString(),
                AccountId = account.AccountID,
                TransactionTypeId = 1,
                Amount = 50,
            };
            RestRequest? restRequest = new(url)
            {
                Method = Method.Post,
            };
            restRequest.AddBody(transactionDto);

            // Act
            RestResponse? response = await this.restClient.ExecuteAsync(restRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            this.data.Entry(account).Reload();
            account.Balance.Should().Be(250);
            var transactionAdded = await this.data.Transactions.FirstAsync();
            transactionAdded.AccountID.Should().Be(account.AccountID);
            transactionAdded.Amount.Should().Be(50);
        }


        [Fact]
        public async Task AddWithdrawalTransactionValidAccount_ReturnsTrue()
        {
            var url = $"{URL}";
            var account = await this.data.Accounts.FirstAsync();
            TransactionDto transactionDto = new()
            {
                TransactionType = Database.TransactionType.Withdrawal.ToString(),
                AccountId = account.AccountID,
                TransactionTypeId = 2,
                Amount = 50,
            };
            RestRequest? restRequest = new(url)
            {
                Method = Method.Post,
            };
            restRequest.AddBody(transactionDto);

            // Act
            RestResponse? response = await this.restClient.ExecuteAsync(restRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            this.data.Entry(account).Reload();
            account.Balance.Should().Be(150);
            var transactionAdded = await this.data.Transactions.FirstAsync();
            transactionAdded.AccountID.Should().Be(account.AccountID);
            transactionAdded.Amount.Should().Be(50);
        }


        [Fact]
        public async Task AddWithdrawalTransactionInvalidAccount_ReturnsFalse()
        {
            var url = $"{URL}";
            TransactionDto transactionDto = new()
            {
                TransactionType = Database.TransactionType.Withdrawal.ToString(),
                AccountId = 2,
                TransactionTypeId = 2,
                Amount = 50,
            };
            RestRequest? restRequest = new(url)
            {
                Method = Method.Post,
            };
            restRequest.AddBody(transactionDto);

            // Act
            RestResponse? response = await this.restClient.ExecuteAsync(restRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
        [Fact]
        public async Task GetTransactionsTypes_ReturnsTrasnactionTypes()
        {
            var url = $"{URL}/types";
            RestRequest? restRequest = new(url)
            {
                Method = Method.Get,
            };

            // Act
            RestResponse? response = await this.restClient.ExecuteAsync(restRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var deserializedResponseContent = JsonSerializer.Deserialize<List<TransactionTypeDto>>(response.Content ?? string.Empty, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, });
            deserializedResponseContent.Should().BeEquivalentTo(new List<TransactionTypeDto>()
            {
                new()
                {
                    TransactionTypeId = 1,
                    Name = Database.TransactionType.Deposit.ToString(),
                },
                new()
                {
                    TransactionTypeId = 2,
                    Name = Database.TransactionType.Withdrawal.ToString(),
                }
            });

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
