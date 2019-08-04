using System;

using JudgeSystem.Common;

using Microsoft.Extensions.DependencyInjection;

namespace JudgeSystem.Web.IocConfiguration
{
    public static class SessionConfiguration
    {
        public static IServiceCollection ConfigureSession(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(GlobalConstants.SessionIdleTimeout);
                options.Cookie.HttpOnly = true;
            });

            return services;
        }
    }
}
