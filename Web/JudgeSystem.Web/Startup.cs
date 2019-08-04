using System;
using System.Reflection;

using JudgeSystem.Common;
using JudgeSystem.Data;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Seeding;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.Dtos.Course;
using JudgeSystem.Web.Dtos.ML;
using JudgeSystem.Web.Filters;
using JudgeSystem.Web.InputModels.Course;
using JudgeSystem.Web.IocConfiguration;
using JudgeSystem.Web.ViewModels;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ML;

namespace JudgeSystem.Web
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddPredictionEnginePool<UserLesson, UserLessonScore>()
                .FromFile(GlobalConstants.LessonsRrecommendationMlModelPath);

            services.ConfigureIdentity();

			services.AddDistributedMemoryCache();
			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromHours(GlobalConstants.SessionIdleTimeout);
				options.Cookie.HttpOnly = true;
			});

            services
                .AddMvc(options => 
				{
                    options.Filters.Add<EntityNotFoundExceptionFilter>();
				})
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddRazorPagesOptions(options =>
                {
                    options.AllowAreas = true;
                    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                });

            services
                .ConfigureApplicationCookie(options =>
                {
                    options.LoginPath = "/Identity/Account/Login";
                    options.LogoutPath = "/Identity/Account/Logout";
                    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                });

            services
                .Configure<CookiePolicyOptions>(options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.Lax;
                    options.ConsentCookie.Name = ".AspNetCore.ConsentCookie";
                });

            services
                .ConfigureSettings(this.configuration)
                .AddEmailSendingService(this.configuration)
                .ConfigureAzureBlobStorage(this.configuration)
                .AddRepositories()
                .AddBusinessLogicServices();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly, 
                typeof(CourseInputModel).GetTypeInfo().Assembly, typeof(ContestCourseDto).GetTypeInfo().Assembly);

			using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (env.IsDevelopment())
                {
                    dbContext.Database.Migrate();
                }

                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
			app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
