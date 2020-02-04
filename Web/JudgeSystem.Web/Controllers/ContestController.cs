using System.Collections.Generic;
using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Services;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.Filters;
using JudgeSystem.Web.Infrastructure.Pagination;
using JudgeSystem.Web.Infrastructure.Routes;
using JudgeSystem.Web.ViewModels.Contest;
using JudgeSystem.Web.ViewModels.Problem;
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
        private readonly IExcelFileGenerator excelFileGenerator;
        private readonly UserManager<ApplicationUser> userManager;

        public ContestController(
            IContestService contestService,
            IExcelFileGenerator excelFileGenerator,
            UserManager<ApplicationUser> userManager)
		{
			this.contestService = contestService;
            this.excelFileGenerator = excelFileGenerator;
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
            var columns = new List<string>
            {
                "№",
                "Class",
                "Full name"
            };

            foreach (ContestProblemViewModel problem in results.Problems)
            {
                columns.Add(problem.Name);
            }

            columns.Add("Total");
            object[,] data = new object[results.ContestResults.Count, columns.Count];
            int col = 0;
            int[] problemIds = results.ProblemIds;

            for (int i = 0; i < results.ContestResults.Count; i++)
            {
                col = 0;
                ContestResultViewModel contestResult = results.ContestResults[i];
                data[i, col++] = contestResult.Student.NumberInCalss;
                data[i, col++] = $"{contestResult.Student.ClassNumber} {contestResult.Student.ClassType}";
                data[i, col++] = contestResult.Student.FullName;
                
                foreach (int problemId in problemIds)
                {
                    if (contestResult.PointsByProblem.TryGetValue(problemId, out int points))
                    {
                        data[i, col++] = points;
                    }
                    else
                    {
                        data[i, col++] = 0;
                    }
                }

                data[i, col++] = contestResult.Total;
            }

            byte[] bytes = excelFileGenerator.Generate(columns, data);
            return File(bytes, GlobalConstants.OctetStreamMimeType, $"{results.Name}.xlsx");
        }
    }
}
