using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Services.Data;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.ViewModels.Problem;
using JudgeSystem.Web.ViewModels.Test;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JudgeSystem.Web.Areas.Administration.Controllers
{
	public class ProblemController : AdministrationBaseController
	{
		private readonly IProblemService problemService;
		private readonly ITestService testService;

		public ProblemController(IProblemService problemService, ITestService testService)
		{
			this.problemService = problemService;
			this.testService = testService;
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(ProblemInputModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			Problem problem = await problemService.Create(model);
			return RedirectToAction(nameof(AddTest), "Problem", 
				new { area = GlobalConstants.AdministrationArea, problemId = problem.Id});
		}

		public IActionResult All(int lessonId)
		{
			var problems = problemService.LesosnProblems(lessonId);
			return View(problems);
		}

		public async Task<IActionResult> Edit(int id)
		{
			Problem problem = await problemService.GetById(id);
			if(problem == null)
			{
				string errorMessage = string.Format(ErrorMessages.NotFoundEntityMessage, "problem");
				return this.ShowError(errorMessage, "All", "Problem", GlobalConstants.AdministrationArea);
			}

			var model = problem.To<Problem, ProblemEditInputModel>();
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(ProblemEditInputModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			Problem problem = await problemService.Update(model);
			return RedirectToAction(nameof(All), "Problem", 
				new { area = GlobalConstants.AdministrationArea, problem.LessonId });
		}

		public async Task<IActionResult> Delete(int id)
		{
			Problem problem = await problemService.GetById(id);
			if (problem == null)
			{
				string errorMessage = string.Format(ErrorMessages.NotFoundEntityMessage, "problem");
				return this.ShowError(errorMessage, "All", "Problem", GlobalConstants.AdministrationArea);
			}

			var model = problem.To<Problem, ProblemViewModel>();
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(ProblemViewModel model)
		{
			Problem problem = await problemService.GetById(model.Id);
			if(problem == null)
			{
				this.ThrowEntityNullException(nameof(problem));
			}

			await problemService.Delete(problem);

			return RedirectToAction(nameof(All), "Problem",
				new { area = GlobalConstants.AdministrationArea, problem.LessonId });
		}

		public IActionResult AddTest()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> AddTest(TestInputModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			await testService.Add(model);
			return RedirectToAction(nameof(AddTest), new { problemId = model.ProblemId });
		}
	}
}
