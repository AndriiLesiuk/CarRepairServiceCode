using CarRepairServiceCode.Repository.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace CarRepairServiceCode.IntegrationTests.TestConfig
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void ConfigureServicesDb(IServiceCollection services)
        {
            services.AddDbContext<CarRepairServiceDB_Context>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly(typeof(TestStartup).GetTypeInfo().Assembly.GetName().Name)
                ));
            services.AddTransient<TestDataSeeder>();
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            base.Configure(app, env, loggerFactory);
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var seeder = serviceScope.ServiceProvider.GetService<TestDataSeeder>();
            seeder.SeedToDoItems();
        }
    }
}
