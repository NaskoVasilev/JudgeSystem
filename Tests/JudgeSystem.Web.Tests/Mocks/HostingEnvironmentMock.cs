using System;
using System.IO;

using Microsoft.AspNetCore.Hosting;
using Moq;

namespace JudgeSystem.Web.Tests.Mocks
{
    public class HostingEnvironmentMock
    {
        private static readonly string WebRootPath = Path.Combine(Environment.CurrentDirectory, "../../../../../Web/JudgeSystem.Web/wwwroot");

        public static IWebHostEnvironment CreateInstance()
            => CreateInstance(WebRootPath);

        public static IWebHostEnvironment CreateInstance(string webRootPath)
        {
            var hostingEnvironmentMock = new Mock<IWebHostEnvironment>();
            hostingEnvironmentMock.Setup(h => h.WebRootPath).Returns(webRootPath);

            return hostingEnvironmentMock.Object;
        }
    }
}
