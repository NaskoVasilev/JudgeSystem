using JudgeSystem.Common.Settings;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        }
    }
}
