namespace JudgeSystem.Web.Controllers
{
	using System.Threading.Tasks;

	using JudgeSystem.Data.Models;
	using JudgeSystem.Services.Data;
	using JudgeSystem.Web.ViewModels.Submission;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Mvc;

	public class SubmissionController : BaseController
	{
		private readonly ISubmissionService submissionService;
		private readonly UserManager<ApplicationUser> userManager;

		public SubmissionController(ISubmissionService submissionService, UserManager<ApplicationUser> userManager)
		{
			this.submissionService = submissionService;
			this.userManager = userManager;
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Create(SubmissionInputModel model)
		{
			string userId = userManager.GetUserId(this.User);
			Submission submission = await submissionService.Create(model, userId);
			return Json(submission);
		}


	}
}
