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
            ConfigureAdminSettings(services, configuration);

            return services;
        }

        private static void ConfigureAdminSettings(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(x =>
            {
                AdminSettings adminSettings = new AdminSettings();
                configuration.GetSection(AppSettingsSections.AdminSection).Bind(adminSettings);
                return adminSettings;
            });
        }
    }
}
