using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Services;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.Filters;
using JudgeSystem.Web.Infrastructure.Pagination;
using JudgeSystem.Web.Infrastructure.Routes;
using JudgeSystem.Web.Resources;
using JudgeSystem.Web.ViewModels.Contest;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace JudgeSystem.Web.Controllers
{
    [Authorize]
    public class ContestController : BaseController
	{
        private const int DefaultPage = 1;
        private const int EntitiesPerPage = 15;

		private readonly IContestService contestService;
        private readonly IExcelFileGenerator excelFileGenerator;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IStringLocalizer<SharedResources> sharedLocalizer;

        public ContestController(
            IContestService contestService,
            IExcelFileGenerator excelFileGenerator,
            UserManager<ApplicationUser> userManager,
            IStringLocalizer<SharedResources> sharedLocalizer)
		{
			this.contestService = contestService;
            this.excelFileGenerator = excelFileGenerator;
            this.userManager = userManager;
            this.sharedLocalizer = sharedLocalizer;
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
            List<string> columns = GenerateColumns(results.Problems.Select(x => x.Name));
            byte[] bytes = excelFileGenerator.GenerateContestResultsReport(results, columns);

            return File(bytes, GlobalConstants.OctetStreamMimeType, $"{results.Name}{GlobalConstants.ExcelFileExtension}");
        }

        private List<string> GenerateColumns(IEnumerable<string> problemNames)
        {
            var columns = new List<string>
            {
                sharedLocalizer[ModelConstants.StudentNumberInClassDisplayName],
                sharedLocalizer[ModelConstants.StudentSchoolClassIdDisplayName],
                sharedLocalizer[ModelConstants.StudentFullNameDisplayName]
            };

            foreach (string name in problemNames)
            {
                columns.Add(name);
            }

            columns.Add(sharedLocalizer[nameof(ContestResultViewModel.Total)]);
            return columns;
        }
    }
}
