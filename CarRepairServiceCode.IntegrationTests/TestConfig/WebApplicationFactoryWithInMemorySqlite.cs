﻿using CarRepairServiceCode.Repository.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace CarRepairServiceCode.IntegrationTests.TestConfig
{
    public class WebApplicationFactoryWithInMemorySqlite : BaseWebApplicationFactory<TestStartup>
    {
        private readonly string _connectionString = "DataSource=:memory:";
        private readonly SqliteConnection _connection;
        public readonly HttpClient _client;

        public WebApplicationFactoryWithInMemorySqlite()
        {
            _connection = new SqliteConnection(_connectionString);
            _connection.Open();
            _client = CreateClient();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder) =>
            builder.ConfigureServices(services =>
            {
                services
                    .AddEntityFrameworkSqlite()
                    .AddDbContext<CarRepairServiceDB_Context>(options =>
                    {
                        options.UseSqlite(_connection);
                        options.UseInternalServiceProvider(services.BuildServiceProvider());
                    });
            });

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _connection.Close();
        }
    }
}
