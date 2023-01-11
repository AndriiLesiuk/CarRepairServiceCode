using CarRepairServiceCode.Filters;
using CarRepairServiceCode.Helper;
using CarRepairServiceCode.Helper.HelperInterfaces;
using CarRepairServiceCode.RabbitMQServices.Interfaces;
using CarRepairServiceCode.RabbitMQServices.RabbitConfig;
using CarRepairServiceCode.RabbitMQServices.Senders;
using CarRepairServiceCode.Repository.Contexts;
using CarRepairServiceCode.Repository.Interfaces;
using CarRepairServiceCode.Repository.Models;
using CarRepairServiceCode.Repository.Repositories;
using CarRepairServiceCode.RequestModels.Authorization;
using CarRepairServiceCode.RequestModels.CarOrder;
using CarRepairServiceCode.RequestModels.CarOrderDetail;
using CarRepairServiceCode.RequestModels.EmpPosition;
using CarRepairServiceCode.RequestModels.TaskCatalog;
using CarRepairServiceCode.Services;
using CarRepairServiceCode.Services.ServiceInterfaces;
using CarRepairServiceCode.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Collections.Generic;
using System.Text;

namespace CarRepairServiceCode
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        protected virtual void ConfigureServicesDb(IServiceCollection services)
        {
            services.AddDbContext<CarRepairServiceDB_Context>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("ConnectionStringPostgres"));
            }, ServiceLifetime.Transient);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IEmpPositionsService, EmpPositionsService>();
            services.AddTransient<IEmpPositionRepository, EmployeePositionRepository>();
            services.AddTransient<Services.ServiceInterfaces.IAuthorizationService, AuthorizationService>();
            services.AddTransient<IAuthorizationRepository, AuthorizationRepository>();
            services.AddTransient<ICarOrderService, CarOrderService>();
            services.AddTransient<ICarOrderRepository, CarOrderRepository>();
            services.AddTransient<ITaskCatalogService, TaskCatalogService>();
            services.AddTransient<ITaskCatalogRepository, TaskCatalogRepository>();
            services.AddTransient<IPermissionRepository, PermissionRepository>();
            services.AddTransient<ITokenHelper, TokenHelper>();
            services.AddTransient<IValidator<AuthRequest>, AuthValidator>();
            services.AddTransient<IValidator<EmpPositionRequest>, EmpPositionValidator>();
            services.AddTransient<IValidator<CarOrderRequest>, CarOrderValidator>();
            services.AddTransient<IValidator<CarOrderDetailRequest>, CarOrderDetailsValidator>();
            services.AddTransient<IValidator<TaskCatalogRequest>, TaskCatalogValidator>();
            services.AddTransient<IAccountingInfoSender, AccountingInfoSender>();

            ConfigureServicesDb(services);

            services.Configure<AuthOptions>(Configuration.GetSection("AuthOptions"));
            services.Configure<RabbitMqConfiguration>(Configuration.GetSection("RabbitMqConfiguration"));

            var authOptions = Configuration.GetSection("AuthOptions").Get<AuthOptions>();

            services.AddMvc(o =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                o.Filters.Add(new AuthorizeFilter(policy));
            }).AddFluentValidation();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "CarRepairService API",
                    Description = "Swagger For CarRepairService API"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Authorization",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

                c.EnableAnnotations();
            });

            services.AddAutoMapper(typeof(AutoMapping.AutoMapping));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ValidIssuer = authOptions.Issuer,
                    ValidAudience = authOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.SecureKey))
                };
            });

            services.AddControllers(options => options.Filters.Add(new ExceptionFilter()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API V1");
            });

            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
