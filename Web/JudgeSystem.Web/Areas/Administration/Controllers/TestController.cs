using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.InputModels.Test;
using JudgeSystem.Web.Filters;
using JudgeSystem.Web.ViewModels.Test;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace JudgeSystem.Web.Areas.Administration.Controllers
{
    public class TestController : AdministrationBaseController
	{
		private readonly ITestService testService;
		private readonly IProblemService problemService;
        private readonly IHostingEnvironment env;

        public TestController(ITestService testService, IProblemService problemService,  IHostingEnvironment env)
		{
			this.testService = testService;
			this.problemService = problemService;
            this.env = env;
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

        public async Task<IActionResult> Edit(int id)
        {
            TestEditInputModel model = await testService.GetById<TestEditInputModel>(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TestEditInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await testService.Update(model);

            return RedirectToAction(nameof(ProblemTests), new { problemId = model.ProblemId });
        }

        [EndpointExceptionFilter]
		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			await testService.Delete(id);
			return Content(string.Format(InfoMessages.SuccessfullyDeletedMessage, "test"));
		}

        public IActionResult DownloadTemplate()
        {
            string filePath = env.WebRootPath + GlobalConstants.AddTestsTemplsteFilePath;
            byte[] bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, GlobalConstants.OctetStreamMimeType, GlobalConstants.AddTestsTemplateFileName);
        }
	}
}
