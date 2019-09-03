using System;
using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Common.Settings;
using JudgeSystem.Data.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace JudgeSystem.Data.Seeding
{
    public class AdminSeeder : ISeeder
	{
		public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
		{
			AdminSettings adminSettings = serviceProvider.GetRequiredService<AdminSettings>();
			UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
			ApplicationUser userFromDb = await userManager.FindByNameAsync(adminSettings.Username);

            if (userFromDb != null)
			{
				return;
			}

			var user = new ApplicationUser
			{
				Name = adminSettings.Name,
				Email = adminSettings.Email,
				UserName = adminSettings.Username,
				Surname = adminSettings.Surname,
                EmailConfirmed = true
			};

			await userManager.CreateAsync(user, adminSettings.Password);
			await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
		}
	}
}
