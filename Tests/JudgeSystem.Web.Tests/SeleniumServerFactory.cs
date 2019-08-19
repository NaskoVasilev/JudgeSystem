﻿using System;
using System.Diagnostics;
using System.Linq;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace JudgeSystem.Web.Tests
{
    public class SeleniumServerFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        private readonly Process process;

        private IWebHost host;

        public SeleniumServerFactory()
        {
            ClientOptions.BaseAddress = new Uri("https://localhost"); // will follow redirects by default

            process = new Process
                       {
                           StartInfo = new ProcessStartInfo
                                       {
                                           FileName = "selenium-standalone",
                                           Arguments = "start",
                                           UseShellExecute = true,
                                       },
                       };
            process.Start();
        }

        public string RootUri { get; set; } // Save this use by tests

        protected override TestServer CreateServer(IWebHostBuilder builder)
        {
            // Real TCP port
            host = builder.Build();
            host.Start();
            RootUri = host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.LastOrDefault(); // Last is https://localhost:5001!

            // Fake Server we won't use...this is lame. Should be cleaner, or a utility class
            return new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                host.Dispose();
                process.CloseMainWindow(); // Be sure to stop Selenium Standalone
            }
        }

        public class FakeStartup
        {
            public void ConfigureServices(IServiceCollection services)
            {
            }

            public void Configure()
            {
            }
        }
    }
}
