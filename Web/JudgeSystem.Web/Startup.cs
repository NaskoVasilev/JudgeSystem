using System.Reflection;

using JudgeSystem.Common;
using JudgeSystem.Data;
using JudgeSystem.Data.Seeding;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.Dtos.Course;
using JudgeSystem.Web.Dtos.ML;
using JudgeSystem.Web.InputModels.Course;
using JudgeSystem.Web.IocConfiguration;
using JudgeSystem.Web.ViewModels;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
                options => options.UseSqlServer(configuration.GetConnectionString(GlobalConstants.DefaultConnectionStringName)));

            services.AddPredictionEnginePool<UserLesson, UserLessonScore>()
                .FromFile(GlobalConstants.LessonsRrecommendationMlModelPath);

            services.ConfigureIdentity()
                .ConfigureSession()
                .ConfigureMvc()
                .ConfigureCookies()
                .ConfigureSettings(configuration)
                .AddEmailSendingService(configuration)
                .ConfigureAzureBlobStorage(configuration)
                .AddRepositories()
                .AddBusinessLogicServices();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly, 
                typeof(CourseInputModel).GetTypeInfo().Assembly, typeof(ContestCourseDto).GetTypeInfo().Assembly);

			using (IServiceScope serviceScope = app.ApplicationServices.CreateScope())
            {
                ApplicationDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

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
