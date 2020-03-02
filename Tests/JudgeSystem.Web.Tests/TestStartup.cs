using JudgeSystem.Common.Settings;
using JudgeSystem.Services;
using JudgeSystem.Web.Tests.Mocks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MyTested.AspNetCore.Mvc;


namespace JudgeSystem.Web.Tests
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public void ConfigureTestServices(IServiceCollection services)
        {
            var adminSettings = new AdminSettings()
            {
                Email = "admin@test.gmail.com",
                Name = "Root",
                Password = "r00t",
                Surname = "Admin",
                Username = "root"
            };
            services.AddSingleton(adminSettings);

            ConfigureServices(services);

            services.Replace<IAzureStorageService, AzureStorageServiceMock>(ServiceLifetime.Transient);
            //Mock IHostingEnvironment
            services.AddScoped(_ => HostingEnvironmentMock.CreateInstance());
        }
    }
}
