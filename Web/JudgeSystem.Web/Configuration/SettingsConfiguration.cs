using JudgeSystem.Common;
using JudgeSystem.Common.Settings;
using JudgeSystem.Workers.Common;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JudgeSystem.Web.Configuration
{
    public static class SettingsConfiguration
    {
        public static IServiceCollection ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(x => configuration.GetSection(AppSettingsSections.AdminSection).Get<AdminSettings>());
            services.AddSettings<WorkerApiSettings>(nameof(WorkerApiSettings), configuration);

            PopulateCompilationSettings(configuration);

            return services;
        }

        public static T AddSettings<T>(this IServiceCollection services, string sectionName, IConfiguration configuration)
           where T : class
        {
            T settings = configuration
               .GetSection(sectionName)
               .Get<T>();

            if (settings != null)
            {
                services.AddSingleton(settings);
            }

            return settings;
        }

        private static void PopulateCompilationSettings(IConfiguration configuration)
        {
            IConfigurationSection compilersSection = configuration.GetSection(AppSettingsSections.CompilersSection);

            CompilationSettings.JavaCompilerPath = compilersSection[nameof(ProgrammingLanguage.Java)];
            CompilationSettings.CppCompilerPath = compilersSection[nameof(ProgrammingLanguage.CPlusPlus)];
            SetWorkingDirectory(configuration);
        }

        private static void SetWorkingDirectory(IConfiguration configuration)
        {
            string workingDirectory = configuration[AppSettingsSections.WorkingDirectory];
            if (!string.IsNullOrEmpty(workingDirectory))
            {
                GlobalConstants.CompilationDirectoryPath = workingDirectory;
            }
        }
    }
}
