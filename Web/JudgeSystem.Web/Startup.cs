using System.Reflection;

using JudgeSystem.Common;
using JudgeSystem.Data;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.Configuration;
using JudgeSystem.Web.Dtos.Course;
using JudgeSystem.Web.Dtos.ML;
using JudgeSystem.Web.Infrastructure.Extensions;
using JudgeSystem.Web.InputModels.Course;
using JudgeSystem.Web.ViewModels;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                .ConfigureDistributedSqlServerCache(configuration)
                .ConfigureLocalization()
                .ConfigureMvc()
                .ConfigureCookies()
                .ConfigureSettings(configuration)
                .AddEmailSendingService(configuration)
                .AddApplicationInsightsTelemetry()
                .ConfigureAzureBlobStorage(configuration)
                .AddRepositories()
                .AddBusinessLogicServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            LocalizationConfiguration.SetDefaultCulture();

            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly,
                typeof(CourseInputModel).GetTypeInfo().Assembly, typeof(ContestCourseDto).GetTypeInfo().Assembly);

            app.UseSeeder();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UserLocalization();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(routes =>
            {
                routes.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
