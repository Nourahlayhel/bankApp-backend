
namespace TransAcc.Test
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.IO;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using TransAccount;
    using TransAccount.Database;

    /// <summary>
    /// The test helpers.
    /// </summary>
    public static class TestHelpers
    {
        /// <summary>
        /// Uses the sqlite.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="databaseName">The database name.</param>
        /// <returns>A WebApplicationFactory.</returns>
        public static WebApplicationFactory<Program> UseSqlite(WebApplicationFactory<Program> factory, string databaseName)
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.UnitTest.json")
            .Build();
            return factory.WithWebHostBuilder(builder =>
            {
                builder.UseConfiguration(config);
                builder.ConfigureServices(services =>
                {
                    services.Remove(services.Single(descriptor => descriptor.ServiceType == typeof(TransAccContext)));
                    services.Remove(services.Single(descriptor => descriptor.ServiceType == typeof(DbContextOptions)));
                    services.Remove(services.Single(descriptor => descriptor.ServiceType == typeof(DbContextOptions<TransAccContext>)));
                    services.AddDbContext<TransAccContext>(options => options.UseSqlite(GetConnectionString(databaseName)));
                });
            });
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <returns>A string.</returns>
        public static string GetConnectionString(string databaseName)
        {
            return $"DataSource=file:{databaseName}?mode=memory";
        }
    }
}