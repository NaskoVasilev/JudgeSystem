using JudgeSystem.Common;
using JudgeSystem.Common.Settings;
using JudgeSystem.Workers.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JudgeSystem.Web.IocConfiguration
{
    public static class SettingsConfiguration
    {
        public static IServiceCollection ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(x => configuration.GetSection(AppSettingsSections.AdminSection).Get<AdminSettings>());

            PopulateCompilationSettings(configuration);

            return services;
        }

        private static void PopulateCompilationSettings(IConfiguration configuration)
        {
            IConfigurationSection compilersSection = configuration.GetSection(AppSettingsSections.CompilersSection);

            CompilationSettings.JavaCompilerPath = compilersSection[nameof(ProgrammingLanguage.Java)];
            CompilationSettings.CppCompilerPath = compilersSection[nameof(ProgrammingLanguage.CPlusPlus)];
        }
    }
}
