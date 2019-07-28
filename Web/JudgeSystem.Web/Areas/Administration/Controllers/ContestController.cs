using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services;
using JudgeSystem.Services.Data;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.Filters;
using JudgeSystem.Web.Infrastructure.Pagination;
using JudgeSystem.Web.InputModels.Contest;
using JudgeSystem.Web.Utilites;
using JudgeSystem.Web.ViewModels.Contest;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JudgeSystem.Web.Areas.Administration.Controllers
{
    public class ContestController : AdministrationBaseController
	{
		private const int DefaultPage = 1;
        private const int SubmissionsPerPage = 2;
		private readonly IContestService contestService;
		private readonly ICourseService courseService;
		private readonly ILessonService lessonService;
        private readonly ISubmissionService submissionService;
        private readonly IProblemService problemService;
        private readonly IPaginationHelper paginationHelper;

        public ContestController(
            IContestService contestService, 
            ICourseService courseService, 
            ILessonService lessonService,
            ISubmissionService submissionService,
            IProblemService problemService,
            IPaginationHelper paginationHelper)
		{
			this.contestService = contestService;
			this.courseService = courseService;
			this.lessonService = lessonService;
            this.submissionService = submissionService;
            this.problemService = problemService;
            this.paginationHelper = paginationHelper;
        }

		public IActionResult Create()
		{
			var courses = courseService.GetAllCourses().Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
			ViewData["courses"] = courses;
			ViewData["lessonTypes"] = Utility.GetSelectListItems<LessonType>();
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(ContestCreateInputModel model)
		{
			if (!ModelState.IsValid)
			{
				var courses = courseService.GetAllCourses().Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
				ViewData["courses"] = courses;
				ViewData["lessonTypes"] = Utility.GetSelectListItems<LessonType>();
				return View(model);
			}

			Contest contest = model.To<ContestCreateInputModel, Contest>();
			await contestService.Create(contest);
			return Redirect("/");
		}

        [EndpointExceptionFilter]
		public IActionResult GetLessons(int courseId, LessonType lessonType)
		{
			var lessons = lessonService.GetCourseLesosns(courseId, lessonType);
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

		[HttpPost]
		public async Task<IActionResult> Edit(ContestEditInputModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			await contestService.UpdateContest(model);
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
			await contestService.DeleteContestById(id);
			return RedirectToAction(nameof(ActiveContests));
		}

		public IActionResult All(int page = DefaultPage)
		{
			int numberOfPages = contestService.GetNumberOfPages();
			IEnumerable<ContestViewModel> contests = contestService.GetAllConests(page);
			ContestAllViewModel model = new ContestAllViewModel { Contests = contests, NumberOfPages = numberOfPages, CurrentPage = page };
			return View(model);
		}

		public IActionResult Results(int id, int? page)
		{
			ViewData["numberOfPages"] = contestService.GetContestResultsPagesCount(id);
			if (page.HasValue)
			{
				ViewData["currentPage"] = page;
				var contestResults = contestService.GetContestReults(id, page.Value);
				return PartialView(contestResults);
			}
			else
			{
				ViewData["currentPage"] = DefaultPage;
				var contestResults = contestService.GetContestReults(id, DefaultPage);
				return View(contestResults);
			}
		}

        [EndpointExceptionFilter]
		[HttpGet("/Contest/Results/{contestId}/PagesCount")]
		public int GetContestResultPagesCount(int contestId)
		{
			return contestService.GetContestResultsPagesCount(contestId);
		}

        public IActionResult Submissions(string userId, int contestId, int? problemId, int page = DefaultPage )
        {
            int baseProblemId;
            if(problemId.HasValue)
            {
                baseProblemId = problemId.Value;
            }
            else
            {
                baseProblemId = contestService.GetFirstProblemId(contestId);
            }

            var submissions = submissionService.GetUserSubmissionsByProblemIdAndContestId(contestId, baseProblemId, userId, page, SubmissionsPerPage);
            string problemName = problemService.GetProblemName(baseProblemId);
            int lessonId = contestService.GetLessonId(contestId);

            string baseUrl = $"/{GlobalConstants.AdministrationArea}/Contest/Submissions?contestId={contestId}&userId={userId}";

            int submissionsCount = submissionService.GetSubmissionsCountByProblemIdAndContestId(baseProblemId, contestId, userId);

            PaginationData paginationData = new PaginationData
            {
                CurrentPage = page,
                NumberOfPages = paginationHelper.CalculatePagesCount(submissionsCount, SubmissionsPerPage),
                Url = baseUrl + $"&problemId={baseProblemId}" + "&page={0}"
            };

            var model = new ContestSubmissionsViewModel
            {
                ProblemName = problemName,
                Submissions = submissions,
                LessonId = lessonId,
                UrlPlaceholder = baseUrl + "&problemId={0}",
                PaginationData = paginationData
            };

            return View(model);
        }
	}
}
