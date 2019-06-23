namespace JudgeSystem.Web.Controllers
{
	using JudgeSystem.Data.Models;

	using Microsoft.AspNetCore.Identity;

	public class ProblemController : BaseController
	{
		private readonly UserManager<ApplicationUser> userManager;

		public ProblemController(UserManager<ApplicationUser> userManager)
		{
			this.userManager = userManager;
		}
	}
}
