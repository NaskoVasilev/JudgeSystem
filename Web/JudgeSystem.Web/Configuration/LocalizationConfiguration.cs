using System.Globalization;

using JudgeSystem.Common;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace JudgeSystem.Web.Configuration
{
    public static class LocalizationConfiguration
    {
        private const string ResourcesFolderName = "Resources";

        public static IServiceCollection ConfigureLocalization(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = ResourcesFolderName);
            return services;
        }

        public static IApplicationBuilder UserLocalization(this IApplicationBuilder app)
        {
            CultureInfo[] supportedCultures = new[]
            {
                new CultureInfo(GlobalConstants.EnglishCultureInfo),
                new CultureInfo(GlobalConstants.CurrentCultureInfo),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(GlobalConstants.CurrentCultureInfo),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            return app;
        }

        public static void SetDefaultCulture()
        {
            var cultureInfo = new CultureInfo(GlobalConstants.CurrentCultureInfo);
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }
    }
}
