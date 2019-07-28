namespace JudgeSystem.Web.Controllers
{
    using JudgeSystem.Common;
    using JudgeSystem.Data.Models;
    using JudgeSystem.Services;
    using JudgeSystem.Services.Data;
    using JudgeSystem.Web.Filters;
    using JudgeSystem.Web.Infrastructure.Pagination;
    using JudgeSystem.Web.ViewModels.Contest;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ContestController : BaseController
	{
        private const int DefaultPage = 1;

		private readonly IContestService contestService;
        private readonly ILessonService lessonService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ISubmissionService submissionService;
        private readonly IProblemService problemService;
        private readonly IPaginationHelper paginationHelper;

        public ContestController(
            IContestService contestService,
            ILessonService lessonService,
            UserManager<ApplicationUser> userManager,
            ISubmissionService submissionService,
            IProblemService problemService,
            IPaginationHelper paginationHelper)
		{
			this.contestService = contestService;
            this.lessonService = lessonService;
            this.userManager = userManager;
            this.submissionService = submissionService;
            this.problemService = problemService;
            this.paginationHelper = paginationHelper;
        }

        [EndpointExceptionFilter]
		[Authorize]
		public int GetNumberOfPages()
		{
			int pagesNumber = contestService.GetNumberOfPages();
			return pagesNumber;
		}

        [Authorize]
        public IActionResult MyResults(int contestId, int? problemId, int page = DefaultPage)
        {
            string userId = userManager.GetUserId(this.User);

            int baseProblemId;
            int lessonId = contestService.GetLessonId(contestId);
            if (problemId.HasValue)
            {
                baseProblemId = problemId.Value;
            }
            else
            {
                baseProblemId = lessonService.GetFirstProblemId(lessonId);
            }

            var submissions = submissionService.GetUserSubmissionsByProblemIdAndContestId(contestId, baseProblemId, userId, page, GlobalConstants.SubmissionPerPage);
            string problemName = problemService.GetProblemName(baseProblemId);

            string baseUrl = $"/Contest/MyResults?contestId={contestId}";

            int submissionsCount = submissionService.GetSubmissionsCountByProblemIdAndContestId(baseProblemId, contestId, userId);

            PaginationData paginationData = new PaginationData
            {
                CurrentPage = page,
                NumberOfPages = paginationHelper.CalculatePagesCount(submissionsCount, GlobalConstants.SubmissionPerPage),
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

            return View($"Areas/{GlobalConstants.AdministrationArea}/Views/Contest/Submissions.cshtml", model);
        }
	}
}
