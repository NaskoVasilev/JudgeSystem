using JudgeSystem.Common;
using JudgeSystem.Common.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JudgeSystem.Web.IocConfiguration
{
    public static class SettingsConfiguration
    {
        public static IServiceCollection ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(x => configuration.GetSection(AppSettingsSections.AdminSection).Get<AdminSettings>());

            return services;
        }

    }
}
