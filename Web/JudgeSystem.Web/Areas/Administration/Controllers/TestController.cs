using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.InputModels.Test;
using JudgeSystem.Web.Filters;
using JudgeSystem.Web.ViewModels.Test;
using JudgeSystem.Web.Utilites;

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
            var model = new ProblemTestsViewModel
            {
                LessonId = await problemService.GetLessonId(problemId),
                Tests = testService.GetTestsByProblemIdOrderedByIsTrialDescending(problemId)
            };
			return View(model);
		}

        [EndpointExceptionFilter]
		[HttpPost]
		public async Task<IActionResult> Edit(TestEditInputModel model)
		{
            if(!ModelState.IsValid)
            {
                string errors = Utility.ExtractModelStateErrors(ModelState, " ");
                return BadRequest(errors);
            }

			await testService.Update(model);
			return Content(string.Format(InfoMessages.SuccessfullyEditMessage, "test"));
		}

        [EndpointExceptionFilter]
		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			await testService.Delete(id);
			return Content(string.Format(InfoMessages.SuccessfullyDeletedMessage, "test"));
		}
	}
}
