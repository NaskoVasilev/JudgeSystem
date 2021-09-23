using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using System.IO;

namespace JudgeSystem.Web
{
    public static class Program
    {
        public static void Main(string[] args) => CreateWebHostBuilder(args).Build().Run();

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseSerilog((hostingContext, configuration)
                => configuration
                    .ReadFrom
                    .Configuration(hostingContext.Configuration)
                    .Enrich
                    .FromLogContext()
                    .WriteTo
                    .File(
                        Path.Combine("./Logs", "log.txt"),
                        rollingInterval: RollingInterval.Day,
                        rollOnFileSizeLimit: true,
                        retainedFileCountLimit: null))
            .UseApplicationInsights()
            .UseStartup<Startup>();
    }
}
