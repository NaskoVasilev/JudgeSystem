using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using JudgeSystem.Checkers;
using JudgeSystem.Compilers;
using JudgeSystem.Data.Models;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.InputModels.Submission;
using JudgeSystem.Workers.Common;
using JudgeSystem.Web.Dtos.Submission;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Web.Utilites;
using JudgeSystem.Web.ViewModels.Submission;
using JudgeSystem.Common;
using JudgeSystem.Web.Filters;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace JudgeSystem.Web.Controllers
{
    [Authorize]
    public class SubmissionController : BaseController
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly ISubmissionService submissionService;
		private readonly ITestService testService;
		private readonly IExecutedTestService executedTestService;
        private readonly IProblemService problemService;

        public SubmissionController(
            UserManager<ApplicationUser> userManager,
            ISubmissionService submissionService, 
			ITestService testService, 
            IExecutedTestService executedTestService,
            IProblemService problemService)
		{
			this.userManager = userManager;
			this.submissionService = submissionService;
			this.testService = testService;
			this.executedTestService = executedTestService;
            this.problemService = problemService;
        }

		public IActionResult Details(int id)
		{
			SubmissionViewModel submission = submissionService.GetSubmissionDetails(id);
			return View(submission);
		}

        public IActionResult Download(int id)
        {
            byte[] submissionCode = submissionService.GetSubmissionCodeById(id);
            string problemName = submissionService.GetProblemNameBySubmissionId(id);

            return this.File(submissionCode, GlobalConstants.OctetStreamMimeType, $"{problemName}.zip");
        }

        [EndpointExceptionFilter]
		public IActionResult GetProblemSubmissions(int problemId, int page = 1, 
            int submissionsPerPage = GlobalConstants.SubmissionsPerPage, int? contestId = null)
		{
			string userId = userManager.GetUserId(this.User);
			IEnumerable<SubmissionResult> submissionResults = new List<SubmissionResult>();
			if (contestId.HasValue)
			{
				submissionResults = submissionService.GetUserSubmissionsByProblemIdAndContestId(contestId.Value, problemId, userId, page, submissionsPerPage);
			}
			else
			{
				submissionResults = submissionService
				.GetUserSubmissionsByProblemId(problemId, userId, page, submissionsPerPage);
			}

			return Json(submissionResults);
		}

        [EndpointExceptionFilter]
        public IActionResult GetSubmissionsCount(int problemId, int? contestId = null)
		{
			string userId = userManager.GetUserId(User);
			int submissionsCount = 0;

			if(contestId.HasValue)
			{
				submissionsCount = submissionService.GetSubmissionsCountByProblemIdAndContestId(problemId, contestId.Value, userId);
			}
			else
			{
				submissionsCount = submissionService.GetProblemSubmissionsCount(problemId, userId);
			}
			return Json(submissionsCount);
		}

        [EndpointExceptionFilter]
        [HttpPost]
        public async Task<IActionResult> Create(SubmissionInputModel model)
        {
            byte[] submissionContent;
            List<string> sourceCodes = new List<string>();
            if (!string.IsNullOrEmpty(model.Code))
            {
                if(model.Code.Length > GlobalConstants.MaxSubmissionCodeLength)
                {
                    return this.BadRequest(ErrorMessages.TooLongSubmissionCode);
                }
                sourceCodes = new List<string> { model.Code };
                submissionContent = Encoding.UTF8.GetBytes(model.Code);
            }
            else if (model.File != null)
            {
                if(Utility.ConvertBytesToKiloBytes(model.File.Length) > GlobalConstants.SubmissionFileMaxSizeInKb)
                {
                    return this.BadRequest(ErrorMessages.TooBigSubmissionFile);
                }

                using(var stream = new System.IO.MemoryStream())
                {
                    await model.File.CopyToAsync(stream);
                    submissionContent = stream.ToArray();
                    sourceCodes = ZipParser.ExtractZipFile(stream);
                }
            }
            else
            {
                return this.BadRequest(ErrorMessages.InvalidSubmission);
            }

            string userId = userManager.GetUserId(this.User);
            model.SubmissionContent = submissionContent;
			Submission submission = await submissionService.Create(model, userId);

			await RunTests(submission, submission.ProblemId, sourceCodes);
			await submissionService.CalculateActualPoints(submission.Id);

			SubmissionResult submissionResult = submissionService.GetSubmissionResult(submission.Id);
			return Json(submissionResult);
		}

		private async Task RunTests(Submission submission, int problemId, List<string> sourceCodes)
		{
			CSharpCompiler compiler = new CSharpCompiler();
			
			CompileResult compileResult = compiler.CreateAssembly(sourceCodes);
			if (!compileResult.IsCompiledSuccessfully)
			{
				submission.CompilationErrors = Encoding.UTF8.GetBytes(string.Join(Environment.NewLine, compileResult.Errors));
				await submissionService.Update(submission);
				return;
			}

			var tests = testService.GetTestsByProblemId(problemId).ToList();
            if(tests.Count == 0)
            {
                return;
            }

			CSharpChecker checker = new CSharpChecker();
            var problemConstraints = problemService.GetProblemConstraints(problemId);
            var memoryLimit = Utility.ConvertMegaBytesToBytes(problemConstraints.AllowedMemoryInMegaBytes) / tests.Count;
            var timeLimit = problemConstraints.AllowedTimeInMilliseconds / tests.Count;

			foreach (var test in tests)
			{
				CheckerResult checkerResult = await checker.Check(compileResult.OutputFile, test.InputData, test.OutputData, timeLimit, memoryLimit);

				ExecutedTest executedTest = new ExecutedTest
				{
					TestId = test.Id,
					SubmissionId = submission.Id,
					ExecutionResultType = Enum.Parse<TestExecutionResultType>(checkerResult.Type.ToString()),
					TimeUsed = checkerResult.TotalProcessorTime.TotalSeconds,
					Output = checkerResult.Output,
					Error = checkerResult.Error,
					MemoryUsed = Utility.ConvertBytesToMegaBytes(checkerResult.MemoryUsed),
					IsCorrect = checkerResult.Type == ProcessExecutionResultType.Success && checkerResult.IsCorrect
				};

				await executedTestService.Create(executedTest);
			}
		}
	}
}
