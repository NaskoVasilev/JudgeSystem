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
	using JudgeSystem.Web.ViewModels.ExecutedTest;
	using JudgeSystem.Web.ViewModels.Submission;
	using JudgeSystem.Workers.Common;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Mvc;

	public class SubmissionController : BaseController
	{
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

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Create(SubmissionInputModel model)
		{
			string userId = userManager.GetUserId(this.User);
			Submission submission = await submissionService.Create(model, userId);

			//TODO make submission compiling and code excution asynchronous
			SubmissionResult submissionResult = await RunTests(submission, submission.ProblemId);
			return Json(submissionResult);
		}

		[NonAction]
		private async Task<SubmissionResult> RunTests(Submission submission, int problemId)
		{
			CSharpCompiler compiler = new CSharpCompiler();
			CompileResult compileResult = compiler.CreateAssembly(Encoding.UTF8.GetString(submission.Code));
			if (!compileResult.IsCompiledSuccessfully)
			{
				submission.CompilationErrors = Encoding.UTF8.GetBytes(string.Join(Environment.NewLine, compileResult.Errors));
				await submissionService.Update(submission);
				return new SubmissionResult { IsCompiledSuccessfully = false };
			}

			IEnumerable<TestDataDto> tests = testService.GetTestsByProblemId(problemId);
			CSharpChecker checker = new CSharpChecker();
			SubmissionResult submissionResult = new SubmissionResult();

			foreach (var test in tests)
			{
				CheckerResult checkerResult = checker.Check(compileResult.OutputFile, test.InputData, test.OutputData);

				ExecutedTest executedTest = new ExecutedTest();
				executedTest.TestId = test.Id;
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

			return submissionResult;
		}
	}
}
