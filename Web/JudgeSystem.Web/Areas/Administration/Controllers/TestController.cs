using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.ViewModels.Test;
using JudgeSystem.Web.InputModels.Test;

using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Areas.Administration.Controllers
{
	public class TestController : AdministrationBaseController
	{
		private readonly ITestService testService;
		private readonly IProblemService problemService;

		public TestController(ITestService testService, IProblemService problemService)
		{
			this.testService = testService;
			this.problemService = problemService;
		}

		public async Task<IActionResult> ProblemTests(int problemId)
		{
			ViewData["lessonId"] = await problemService.GetLessonId(problemId);
			var tests = testService.TestsByProblem(problemId);
			return View(tests);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(TestEditInputModel model)
		{
			Test test = await testService.GetById(model.Id);
			if(test == null)
			{
				return BadRequest(string.Format(ErrorMessages.NotFoundEntityMessage, nameof(test)));
			}

			test.InputData = model.InputData.Trim();
			test.OutputData = model.OutputData.Trim();

			await testService.Update(test);
			return Content(string.Format(InfoMessages.SuccessfullyEditMessage, nameof(test)));
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			Test test = await testService.GetById(id);
			if(test == null)
			{
				return BadRequest(string.Format(ErrorMessages.NotFoundEntityMessage, nameof(test)));
			}

			await testService.Delete(test);
			return Content(string.Format(InfoMessages.SuccessfullyDeletedMessage, nameof(test)));
		}
	}
}
