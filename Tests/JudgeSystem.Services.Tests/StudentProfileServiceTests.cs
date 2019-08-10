using System.Threading.Tasks;
using JudgeSystem.Common;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;

using Moq;
using Xunit;

namespace JudgeSystem.Services.Tests
{
    public class StudentProfileServiceTests
    {
        [Fact]
        public async Task SendActivationEmail_WithGivenEmailAddress_ShouldInvokeIEmailSenderMethodSendEmailAsync()
        {
            string email = "nasko01_vasilev@abv.bg";
            string subject = GlobalConstants.StudentProfileActivationEmailSubject;
            string webRootPath = "../../../../../Web/JudgeSystem.Web/wwwroot";
            var hostingEnvironmentMock = new Mock<IHostingEnvironment>();
            hostingEnvironmentMock.Setup(x => x.WebRootPath).Returns(webRootPath);

            var emailSenderMock = new Mock<IEmailSender>();
            emailSenderMock.Setup(x => x.SendEmailAsync(email, subject, It.IsAny<string>()));

            var service = new StudentProfileService(hostingEnvironmentMock.Object, emailSenderMock.Object);
            var activationKey = await service.SendActivationEmail(email);

            emailSenderMock.Verify(x => x.SendEmailAsync(email, subject, It.Is<string>(m => m.Contains(activationKey))), Times.Once());
        }
    }
}
