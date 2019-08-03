using System.Collections.Generic;
using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Services.Data;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.ViewModels.Problem;
using JudgeSystem.Web.InputModels.Test;
using JudgeSystem.Web.InputModels.Problem;

using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Areas.Administration.Controllers
{
    public class ProblemController : AdministrationBaseController
	{
		private readonly IProblemService problemService;
		private readonly ITestService testService;
        private readonly IPracticeService practiceService;
        private readonly ILessonService lessonService;

        public ProblemController(
            IProblemService problemService, 
            ITestService testService, 
            IPracticeService practiceService,
            ILessonService lessonService)
		{
			this.problemService = problemService;
			this.testService = testService;
            this.practiceService = practiceService;
            this.lessonService = lessonService;
        }

		public IActionResult Create()
		{
			return View(new ProblemInputModel());
		}

        [ValidateAntiForgeryToken]
		[HttpPost]
		public async Task<IActionResult> Create(ProblemInputModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var problem = await problemService.Create(model);
			return RedirectToAction(nameof(AddTest), "Problem",
				new { area = GlobalConstants.AdministrationArea, problemId = problem.Id });
		}

		public IActionResult All(int lessonId)
		{
            IEnumerable<LessonProblemViewModel> problems = problemService.LessonProblems(lessonId);
            int practiceId = lessonService.GetPracticeId(lessonId);
            var model = new ProblemAllViewModel
            {
                Problems = problems,
                LesosnId = lessonId,
                PracticeId = practiceId
            };
			return View(model);
		}

		public async Task<IActionResult> Edit(int id)
		{
			var model = await problemService.GetById<ProblemEditInputModel>(id);
			return View(model);
		}

        [ValidateAntiForgeryToken]
		[HttpPost]
		public async Task<IActionResult> Edit(ProblemEditInputModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var problem = await problemService.Update(model);
			return RedirectToAction(nameof(All), "Problem",
				new { area = GlobalConstants.AdministrationArea, problem.LessonId });
		}

		public async Task<IActionResult> Delete(int id)
		{
			var model = await problemService.GetById<ProblemViewModel>(id);
			return View(model);
		}

        [ValidateAntiForgeryToken]
		[HttpPost]
        [ActionName(nameof(Delete))]
		public async Task<IActionResult> DeletePost(int id)
		{
			var problem = await problemService.Delete(id);

            return RedirectToAction(nameof(All), "Problem",
                new { area = GlobalConstants.AdministrationArea, problem.LessonId });
        }

		public async Task<IActionResult> AddTest(int problemId)
		{
            var model = new TestInputModel { LessonId = await problemService.GetLessonId(problemId) };
			return View(model);
		}

        [ValidateAntiForgeryToken]
		[HttpPost]
		public async Task<IActionResult> AddTest(TestInputModel model)
		{
			if (!ModelState.IsValid)
			{
                model.LessonId = await problemService.GetLessonId(model.ProblemId);
                return View(model);
			}

			await testService.Add(model);
			return RedirectToAction(nameof(AddTest), new { problemId = model.ProblemId });
		}
	}
}
