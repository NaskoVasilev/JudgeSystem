using System.Collections.Generic;
using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.Dtos.Lesson;
using JudgeSystem.Web.Filters;
using JudgeSystem.Web.InputModels.Contest;
using JudgeSystem.Web.ViewModels.Contest;

using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Areas.Administration.Controllers
{
    public class ContestController : AdministrationBaseController
	{
		private const int DefaultPage = 1;

		private readonly IContestService contestService;
		private readonly ILessonService lessonService;

        public ContestController(
            IContestService contestService, 
            ILessonService lessonService)
		{
			this.contestService = contestService;
			this.lessonService = lessonService;
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(ContestCreateInputModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			await contestService.Create(model);

			return Redirect("/");
		}

        [EndpointExceptionFilter]
		public IActionResult GetLessons(int courseId, LessonType lessonType)
		{
            IEnumerable<ContestLessonDto> lessons = lessonService.GetCourseLesosns(courseId, lessonType);
			return Json(lessons);
		}

		public IActionResult ActiveContests()
		{
			IEnumerable<ContestBreifInfoViewModel> activeContests = contestService.GetActiveAndFollowingContests();
			return View(activeContests);
		}

		public async Task<IActionResult> Details(int id)
		{
			ContestViewModel contest = await contestService.GetById<ContestViewModel>(id);
			return View(contest);
		}

		public async Task<IActionResult> Edit(int id)
		{
			ContestEditInputModel contest = await contestService.GetById<ContestEditInputModel>(id);
			return View(contest);
		}

        [ValidateAntiForgeryToken]
		[HttpPost]
		public async Task<IActionResult> Edit(ContestEditInputModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			await contestService.Update(model);

			return RedirectToAction(nameof(ActiveContests));
		}

		public async Task<IActionResult> Delete(int id)
		{
			ContestViewModel contest = await contestService.GetById<ContestViewModel>(id);
			return View(contest);
		}

		[HttpPost]
		[ActionName(nameof(Delete))]
		public async Task<IActionResult> DeletePost(int id)
		{
			await contestService.Delete(id);
			return RedirectToAction(nameof(ActiveContests));
		}

		public IActionResult All(int page = DefaultPage)
		{
			IEnumerable<ContestViewModel> contests = contestService.GetAllConests(page);
            int numberOfPages = contestService.GetNumberOfPages();
			var model = new ContestAllViewModel { Contests = contests, NumberOfPages = numberOfPages, CurrentPage = page };
			return View(model);
		}

		public IActionResult Results(int id, int page = DefaultPage)
		{
            ContestAllResultsViewModel model = contestService.GetContestReults(id, page);
            return View(model);
        }

        [EndpointExceptionFilter]
        [HttpGet("/Contest/Results/{contestId}/PagesCount")]
        public int GetContestResultPagesCount(int contestId) => 
            contestService.GetContestResultsPagesCount(contestId);

        public async Task<IActionResult> Submissions(string userId, int contestId, int? problemId, int page = DefaultPage )
        {
            string baseUrl = $"/{GlobalConstants.AdministrationArea}/Contest/{nameof(Submissions)}?contestId={contestId}&userId={userId}";

            ContestSubmissionsViewModel model = await contestService.GetContestSubmissions(contestId, userId, problemId, page, baseUrl);

            return View(model);
        }
	}
}
