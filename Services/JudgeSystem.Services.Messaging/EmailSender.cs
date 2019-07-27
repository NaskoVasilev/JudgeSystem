using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace JudgeSystem.Services.Messaging
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<SendGridOptions> sendGridOptions, IOptions<BaseEmailOptions> emailOptions)
        {
            SendGridOptions = sendGridOptions.Value;
            EmailOptions = emailOptions.Value;
        }

        public SendGridOptions SendGridOptions{ get; }

        public BaseEmailOptions EmailOptions { get; }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(SendGridOptions.SendGridKey, subject, message, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(this.EmailOptions.Username, this.EmailOptions.Fullname),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}
