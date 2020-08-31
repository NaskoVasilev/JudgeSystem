using JudgeSystem.Web.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace JudgeSystem.Web.Configuration
{
    public static class MvcConfiguration
    {
        private const int EntityNotFoundExceptionFilterOrder = 2;

        public static IServiceCollection ConfigureMvc(this IServiceCollection services)
        {
            services
                .AddMvc(options =>
                {
                    options.Filters.Add<EntityNotFoundExceptionFilter>(EntityNotFoundExceptionFilterOrder);
                })
                .AddViewLocalization()
                .AddMvcLocalization()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                });

            return services;
        }
    }
}
