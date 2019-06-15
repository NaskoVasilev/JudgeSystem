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
		private readonly ITestService testService;

		public SubmissionController(ISubmissionService submissionService, UserManager<ApplicationUser> userManager,
			ITestService testService)
		{
			this.submissionService = submissionService;
			this.userManager = userManager;
			this.testService = testService;
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Create(SubmissionInputModel model)
		{
			string userId = userManager.GetUserId(this.User);
			Submission submission = await submissionService.Create(model, userId);

			return Json(submission);
		}

		//private SubmissionResult RunTests(Submission submission, int problemId)
		//{
		//	CSharpCompiler compiler = new CSharpCompiler();
		//	CompileResult compileResult = compiler.CreateAssembly(Encoding.UTF8.GetString(submission.Code));
		//	if (!compileResult.IsCompiledSuccessfully)
		//	{
		//		return new SubmissionResult { IsCompiledSuccessfully = false };
		//	}

		//	var tests = testService.GetTestsByProblemId(problemId);
		//	foreach (var test in tests)
		//	{
		//		//TODO run all tests
		//	}

		//	return new SubmissionResult();
		//}
	}
}
