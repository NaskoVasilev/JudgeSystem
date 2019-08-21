using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.Filters;
using JudgeSystem.Web.ViewModels.Contest;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Controllers
{
    public class ContestController : BaseController
	{
        private const int DefaultPage = 1;

		private readonly IContestService contestService;
        private readonly UserManager<ApplicationUser> userManager;

        public ContestController(
            IContestService contestService,
            UserManager<ApplicationUser> userManager)
		{
			this.contestService = contestService;
            this.userManager = userManager;
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
            string userId = userManager.GetUserId(User);
            string baseUrl = $"/Contest/MyResults?contestId={contestId}";

            ContestSubmissionsViewModel model = await contestService.GetContestSubmissions(contestId, userId, problemId, page, baseUrl);

            return View($"Areas/{GlobalConstants.AdministrationArea}/Views/Contest/Submissions.cshtml", model);
        }
	}
}
