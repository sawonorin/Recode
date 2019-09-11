using Hangfire;
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recode.Data;

namespace Recode.Api.Extensions
{
    public static class DatabaseExtensions
    {
        public static void AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionstring = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContextPool<APPContext>((serviceProvider, optionsBuilder) =>
            {
                optionsBuilder.UseSqlServer(connectionstring).EnableSensitiveDataLogging();
                optionsBuilder.UseInternalServiceProvider(serviceProvider);
            });

            //enabling hangfire
            services.AddHangfire(config =>
            {
                var options = new SqlServerStorageOptions
                {
                    PrepareSchemaIfNecessary = false,
                    QueuePollInterval = TimeSpan.FromMinutes(5)
                };
                config.UseSqlServerStorage(connectionstring);
            });

            services.AddEntityFrameworkSqlServer();
            services.AddScoped<DbContext, APPContext>();
        }
    }
}
