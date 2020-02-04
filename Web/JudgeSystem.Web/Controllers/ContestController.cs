﻿using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.Filters;
using JudgeSystem.Web.Infrastructure.Pagination;
using JudgeSystem.Web.Infrastructure.Routes;
using JudgeSystem.Web.ViewModels.Contest;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Controllers
{
    [Authorize]
    public class ContestController : BaseController
	{
        private const int DefaultPage = 1;
        private const int EntitiesPerPage = 15;

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
		public int GetNumberOfPages()
		{
			int pagesNumber = contestService.GetNumberOfPages();
			return pagesNumber;
		}

        public async Task<IActionResult> MyResults(int contestId, int? problemId, int page = DefaultPage)
        {
            string userId = userManager.GetUserId(User);
            string baseUrl = $"/Contest/MyResults?contestId={contestId}";

            ContestSubmissionsViewModel model = await contestService.GetContestSubmissions(contestId, userId, problemId, page, baseUrl);

            return View($"Areas/{GlobalConstants.AdministrationArea}/Views/Contest/Submissions.cshtml", model);
        }

        public IActionResult Results(int id, int page = DefaultPage)
        {
            ContestAllResultsViewModel model = contestService.GetContestReults(id, page, EntitiesPerPage);

            var routeString = new RouteString(GlobalConstants.AdministrationArea, nameof(ContestController), nameof(Results));
            model.PaginationData = new PaginationData() 
            { 
                Url = routeString.AppendId(id).AppendPaginationPlaceholder(), 
                NumberOfPages = contestService.GetContestResultsPagesCount(id, EntitiesPerPage), 
                CurrentPage = page 
            };

            return View(model);
        }

        [EndpointExceptionFilter]
        [HttpGet("/Contest/Results/{contestId}/PagesCount")]
        public int GetContestResultPagesCount(int contestId) =>
                    contestService.GetContestResultsPagesCount(contestId, EntitiesPerPage);

        public IActionResult ExportResults(int id)
        {
            ContestAllResultsViewModel results = contestService.GetContestReults(id, DefaultPage, int.MaxValue);
            return null;
        }
    }
}
