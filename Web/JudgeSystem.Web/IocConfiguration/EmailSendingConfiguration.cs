using JudgeSystem.Common;
using JudgeSystem.Services.Messaging;

using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JudgeSystem.Web.IocConfiguration
{
    public static class EmailSendingConfiguration
    {
        public static IServiceCollection AddEmailSendingService(this IServiceCollection services, IConfiguration configuration)
        {
            IConfigurationSection sendGridSection = configuration.GetSection(AppSettingsSections.SendGridSection);
            services.Configure<SendGridOptions>(sendGridSection);

            IConfigurationSection emailSection = configuration.GetSection(AppSettingsSections.EmailSection);
            services.Configure<BaseEmailOptions>(emailSection);

            services.AddTransient<IEmailSender, EmailSender>();

            return services;
        }
    }
}
