using JudgeSystem.Data;
using JudgeSystem.Data.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.Extensions.DependencyInjection;

namespace JudgeSystem.Web.Configuration
{
    public static class IdentityConfiguration
    {
        public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
        {
            services
               .AddIdentity<ApplicationUser, ApplicationRole>(options =>
               {
                   options.Password.RequireDigit = false;
                   options.Password.RequireLowercase = false;
                   options.Password.RequireUppercase = false;
                   options.Password.RequireNonAlphanumeric = false;
                   options.Password.RequiredLength = 6;

                   options.SignIn.RequireConfirmedEmail = true;
               })
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddUserStore<ApplicationUserStore>()
               .AddRoleStore<ApplicationRoleStore>()
               .AddDefaultTokenProviders()
               .AddDefaultUI(UIFramework.Bootstrap4);

            services.AddTransient<IUserStore<ApplicationUser>, ApplicationUserStore>();
            services.AddTransient<IRoleStore<ApplicationRole>, ApplicationRoleStore>();

            return services;
        }
    }
}
