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
	using JudgeSystem.Web.Dtos.ExecutedTest;
	using JudgeSystem.Services;
	using System.Linq;

	public class SubmissionController : BaseController
	{
		private const int SubmissionPerPage = 5;
		private readonly ISubmissionService submissionService;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly ITestService testService;
		private readonly IExecutedTestService executedTestService;
		private readonly IEstimator estimator;

		public SubmissionController(ISubmissionService submissionService, UserManager<ApplicationUser> userManager,
			ITestService testService, IExecutedTestService executedTestService, IEstimator estimator)
		{
			this.submissionService = submissionService;
			this.userManager = userManager;
			this.testService = testService;
			this.executedTestService = executedTestService;
			this.estimator = estimator;
		}

		public IActionResult GetProblemSubmissions(int problemId, int page = 1, int submissionsPerPage = SubmissionPerPage)
		{
			string userId = userManager.GetUserId(this.User);
			IEnumerable<SubmissionResult> submissionResults = submissionService
				.GetUserSubmissionsByProblemId(problemId, userId, page, submissionsPerPage);

			return Json(submissionResults);
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Create(SubmissionInputModel model)
		{
			string userId = userManager.GetUserId(this.User);
			Submission submission = await submissionService.Create(model, userId);
			SubmissionResult submissionResult = submissionService.GetSubmissionResult(submission.Id);

			//TODO make submission compiling and code excution asynchronous
			await RunTests(submissionResult, submission, submission.ProblemId);

			int passedTests = submissionResult.ExecutedTests.Count(t => t.IsCorrect);
			submissionResult.ActualPoints = estimator.CalculteProblemPoints(submissionResult.ExecutedTests.Count,
				passedTests, submissionResult.MaxPoints);

			return Json(submissionResult);
		}

		[NonAction]
		private async Task RunTests(SubmissionResult submissionResult, Submission submission, int problemId)
		{
			CSharpCompiler compiler = new CSharpCompiler();
			CompileResult compileResult = compiler.CreateAssembly(Encoding.UTF8.GetString(submission.Code));
			if (!compileResult.IsCompiledSuccessfully)
			{
				submission.CompilationErrors = Encoding.UTF8.GetBytes(string.Join(Environment.NewLine, compileResult.Errors));
				await submissionService.Update(submission);
				submissionResult.IsCompiledSuccessfully = false;
				return;
			}

			IEnumerable<TestDataDto> tests = testService.GetTestsByProblemId(problemId);
			CSharpChecker checker = new CSharpChecker();

			foreach (var test in tests)
			{
				CheckerResult checkerResult = checker.Check(compileResult.OutputFile, test.InputData, test.OutputData);

				ExecutedTest executedTest = new ExecutedTest();
				executedTest.TestId = test.Id;
				executedTest.SubmissionId = submission.Id;
				ExecutedTestResult executedTestResult = new ExecutedTestResult();

				if (checkerResult.HasRuntimeError)
				{
					executedTest.Output = checkerResult.ErrorMessage;
					executedTest.ExecutedSuccessfully = false;
					executedTestResult.ExecutedSuccessfully = false;
				}
				else
				{
					executedTest.Output = checkerResult.Output;
					executedTest.IsCorrect = checkerResult.IsCorrect;
					executedTestResult.IsCorrect = checkerResult.IsCorrect;
				}

				await executedTestService.Create(executedTest);
				submissionResult.ExecutedTests.Add(executedTestResult);
			}
		}
	}
}
