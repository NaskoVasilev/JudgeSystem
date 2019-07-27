using JudgeSystem.Data.Models;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Controllers
{
	public class UserController : BaseController
	{
		private readonly IUserService userService;
		private readonly UserManager<ApplicationUser> userManager;

		public UserController(IUserService userService, UserManager<ApplicationUser> userManager)
		{
			this.userService = userService;
			this.userManager = userManager;
		}

		[Authorize]
		public IActionResult Results()
		{
			string userId = userManager.GetUserId(this.User);
            UserResultsViewModel userResults = new UserResultsViewModel
            {
                ContestResults = userService.GetContestResults(userId),
                PracticeResults = userService.GetPracticetResults(userId)
            };
            return View(userResults);
		}
	}
}
