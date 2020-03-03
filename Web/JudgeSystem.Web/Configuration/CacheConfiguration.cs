using System;

using JudgeSystem.Common;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JudgeSystem.Web.Configuration
{
    public static class CacheConfiguration
    {
        private const int SessionIdleTimeout = 2;
        public const string SchemaName = "dbo";
        public const string TableName = "Cache";

        public static IServiceCollection ConfigureSession(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(SessionIdleTimeout);
                options.Cookie.HttpOnly = true;
            });

            return services;
        }

        public static IServiceCollection ConfigureDistributedSqlServerCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = configuration[GlobalConstants.DefaultConnectionStringName];
                options.SchemaName = SchemaName;
                options.TableName = TableName;
            });

            return services;
        }
    }
}
