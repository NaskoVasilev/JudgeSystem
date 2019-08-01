using JudgeSystem.Services.Messaging;

using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JudgeSystem.Web.IocConfiguration
{
    public static class EmailSendingConfiguration
    {
        private const string SendGridOptionsKey = "SendGrid";
        private const string EmailOptionsKey = "Email";

        public static IServiceCollection AddEmailSendingService(this IServiceCollection services, IConfiguration configuration)
        {
            var sendGridSection = configuration.GetSection(SendGridOptionsKey);
            services.Configure<SendGridOptions>(sendGridSection);

            var emailSection = configuration.GetSection(EmailOptionsKey);
            services.Configure<BaseEmailOptions>(emailSection);

            services.AddTransient<IEmailSender, EmailSender>();

            return services;
        }
    }
}
