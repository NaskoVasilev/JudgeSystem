using System.Linq;
using System.Collections.Generic;

using JudgeSystem.Common;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.Infrastructure.Pagination;
using JudgeSystem.Web.ViewModels.Practice;
using JudgeSystem.Web.Infrastructure.Routes;
using JudgeSystem.Web.Resources;
using JudgeSystem.Web.Filters;
using JudgeSystem.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace JudgeSystem.Web.Controllers
{
    public class PracticeController : BaseController
    {
        public const int ResultsPerPage = 15;

        private readonly IPracticeService practiceService;
        private readonly IStringLocalizer<SharedResources> sharedLocalizer;
        private readonly IExcelFileGenerator excelFileGenerator;

        public PracticeController(
            IPracticeService practiceService, 
            IStringLocalizer<SharedResources> sharedLocalizer,
            IExcelFileGenerator excelFileGenerator)
        {
            this.practiceService = practiceService;
            this.sharedLocalizer = sharedLocalizer;
            this.excelFileGenerator = excelFileGenerator;
        }

        public IActionResult Results(int id, int page = GlobalConstants.DefaultPage)
        {
            PracticeAllResultsViewModel model = practiceService.GetPracticeResults(id, page, ResultsPerPage);
            var routeString = new RouteString(nameof(PracticeController), nameof(Results));

            model.PaginationData = new PaginationData
            {
                Url = routeString.AppendId(id).AppendPaginationPlaceholder(),
                NumberOfPages = practiceService.GetPracticeResultsPagesCount(id, ResultsPerPage),
                CurrentPage = page
            };

            return View(model);
        }

        public IActionResult ExportResults(int id)
        {
            PracticeAllResultsViewModel results = practiceService.GetPracticeResults(id, GlobalConstants.DefaultPage, int.MaxValue);
            List<string> columns = GenerateColumns(results.Problems.Select(x => x.Name));
            byte[] bytes = excelFileGenerator.GeneratePracticeResultsReport(results, columns);

            return File(bytes, GlobalConstants.OctetStreamMimeType, $"{results.LessonName}{GlobalConstants.ExcelFileExtension}");
        }

        [EndpointExceptionFilter]
        public int ResultsPagesCount(int id) => practiceService.GetPracticeResultsPagesCount(id, ResultsPerPage);

        private List<string> GenerateColumns(IEnumerable<string> problemNames)
        {
            var columns = new List<string>
            {
                sharedLocalizer[ModelConstants.StudentFullNameDisplayName],
                sharedLocalizer[nameof(PracticeResultViewModel.Username)],

            };

            foreach (string name in problemNames)
            {
                columns.Add(name);
            }

            columns.Add(sharedLocalizer[nameof(PracticeResultViewModel.Total)]);
            return columns;
        }
    }
}
