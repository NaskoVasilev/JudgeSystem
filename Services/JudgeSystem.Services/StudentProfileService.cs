using System;
using System.IO;
using System.Threading.Tasks;

using JudgeSystem.Common;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace JudgeSystem.Services
{
    public class StudentProfileService : IStudentProfileService
    {
        private readonly IHostingEnvironment environment;
        private readonly IEmailSender emailSender;

        public StudentProfileService(
            IHostingEnvironment environment,
            IEmailSender emailSender)
        {
            this.environment = environment;
            this.emailSender = emailSender;
        }

        public async Task<string> SendActivationEmail(string email, string baseUrl)
        {
            string activationKey = Guid.NewGuid().ToString();
            string activationKeyPlaceholder = "@{activationKey}";
            string baseUrlPlaceholder = "@{baseUrl}";
            string subject = GlobalConstants.StudentProfileActivationEmailSubject;
            string message = await ReadEmailTemplateAsync();
            message = message.Replace(activationKeyPlaceholder, activationKey);
            message = message.Replace(baseUrlPlaceholder, baseUrl);

            await emailSender.SendEmailAsync(email, subject, message);
            return activationKey;
        }

        private async Task<string> ReadEmailTemplateAsync()
        {
            string activationTemplateName = "StudentProfileActivation.html";
            string path = Path.Combine(environment.WebRootPath, GlobalConstants.TemplatesFolder,
                GlobalConstants.EmailTemplatesFolder, activationTemplateName);

            string temaplte = await Task.Run(() => File.ReadAllText(path));
            return temaplte;
        }
    }
}
