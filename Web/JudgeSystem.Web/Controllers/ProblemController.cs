using JudgeSystem.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Controllers
{
	public class ProblemController : BaseController
	{
		private readonly UserManager<ApplicationUser> userManager;

		public ProblemController(UserManager<ApplicationUser> userManager)
		{
			this.userManager = userManager;
		}
	}
}
