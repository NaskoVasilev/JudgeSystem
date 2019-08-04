using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Services;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.Filters;
using JudgeSystem.Web.Utilites;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Controllers
{
    public class ContestController : BaseController
	{
        private const int DefaultPage = 1;

		private readonly IContestService contestService;
        private readonly ILessonService lessonService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ISubmissionService submissionService;
        private readonly IProblemService problemService;
        private readonly IPaginationService paginationHelper;
        private readonly ContestReslutsHelper contestReslutsHelper;

        public ContestController(
            IContestService contestService,
            ILessonService lessonService,
            UserManager<ApplicationUser> userManager,
            ISubmissionService submissionService,
            IProblemService problemService,
            IPaginationService paginationHelper,
            ContestReslutsHelper contestReslutsHelper)
		{
			this.contestService = contestService;
            this.lessonService = lessonService;
            this.userManager = userManager;
            this.submissionService = submissionService;
            this.problemService = problemService;
            this.paginationHelper = paginationHelper;
            this.contestReslutsHelper = contestReslutsHelper;
        }

        [EndpointExceptionFilter]
		[Authorize]
		public int GetNumberOfPages()
		{
			int pagesNumber = contestService.GetNumberOfPages();
			return pagesNumber;
		}

        [Authorize]
        public async Task<IActionResult> MyResults(int contestId, int? problemId, int page = DefaultPage)
        {
            string userId = userManager.GetUserId(this.User);
            string baseUrl = $"/Contest/MyResults?contestId={contestId}";

            var model = await contestReslutsHelper.GetContestSubmissions(contestId, userId, problemId, page, baseUrl);

            return View($"Areas/{GlobalConstants.AdministrationArea}/Views/Contest/Submissions.cshtml", model);
        }
	}
}
