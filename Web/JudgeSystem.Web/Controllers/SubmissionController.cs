namespace JudgeSystem.Web.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Threading.Tasks;
	using JudgeSystem.Checkers;
	using JudgeSystem.Compilers;
	using JudgeSystem.Data.Models;
	using JudgeSystem.Services.Data;
	using JudgeSystem.Web.Dtos.Test;
	using JudgeSystem.Web.InputModels.Submission;
	using JudgeSystem.Workers.Common;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Mvc;
	using JudgeSystem.Web.Dtos.Submission;
	using JudgeSystem.Data.Models.Enums;
	using JudgeSystem.Web.Utilites;
	using JudgeSystem.Web.ViewModels.Submission;

	public class SubmissionController : BaseController
	{
		private const int SubmissionPerPage = 5;
		private readonly ISubmissionService submissionService;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly ITestService testService;
		private readonly IExecutedTestService executedTestService;

		public SubmissionController(ISubmissionService submissionService, UserManager<ApplicationUser> userManager,
			ITestService testService, IExecutedTestService executedTestService)
		{
			this.submissionService = submissionService;
			this.userManager = userManager;
			this.testService = testService;
			this.executedTestService = executedTestService;
		}

		public IActionResult Details(int id)
		{
			SubmissionViewModel submission = submissionService.GetSubmissionDetails(id);
			return View(submission);
		}

		public IActionResult GetProblemSubmissions(int problemId, int page = 1, int submissionsPerPage = SubmissionPerPage)
		{
			string userId = userManager.GetUserId(this.User);
			IEnumerable<SubmissionResult> submissionResults = submissionService
				.GetUserSubmissionsByProblemId(problemId, userId, page, submissionsPerPage);

			return Json(submissionResults);
		}

		public IActionResult GetSubmissionsCount(int problemId)
		{
			string userId = userManager.GetUserId(User);
			int submissionsCount = submissionService.GetProblemSubmissionsCount(problemId, userId);
			return Json(submissionsCount);
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Create(SubmissionInputModel model)
		{
			string userId = userManager.GetUserId(this.User);
			Submission submission = await submissionService.Create(model, userId);

			//TODO make submission compiling and code excution asynchronous
			await RunTests(submission, submission.ProblemId);

			SubmissionResult submissionResult = submissionService.GetSubmissionResult(submission.Id);

			return Json(submissionResult);
		}

		[NonAction]
		private async Task RunTests(Submission submission, int problemId)
		{
			CSharpCompiler compiler = new CSharpCompiler();
			//TODO: run compilation asynchronously
			CompileResult compileResult = compiler.CreateAssembly(Encoding.UTF8.GetString(submission.Code));
			if (!compileResult.IsCompiledSuccessfully)
			{
				submission.CompilationErrors = Encoding.UTF8.GetBytes(string.Join(Environment.NewLine, compileResult.Errors));
				await submissionService.Update(submission);
				return;
			}

			IEnumerable<TestDataDto> tests = testService.GetTestsByProblemId(problemId);
			CSharpChecker checker = new CSharpChecker();

			foreach (var test in tests)
			{
				CheckerResult checkerResult = await checker.Check(compileResult.OutputFile, test.InputData, test.OutputData);

				ExecutedTest executedTest = new ExecutedTest
				{
					TestId = test.Id,
					SubmissionId = submission.Id,
					ExecutionResultType = Enum.Parse<TestExecutionResultType>(checkerResult.Type.ToString()),
					TimeUsed = checkerResult.TotalProcessorTime.TotalSeconds,
					Output = checkerResult.Output,
					Error = checkerResult.Error,
					MemoryUsed = Utility.ConvertBytesToMegaBytes(checkerResult.MemoryUsed),
					IsCorrect = checkerResult.Type == ProcessExecutionResultType.Success
				};

				await executedTestService.Create(executedTest);
			}
		}
	}
}
