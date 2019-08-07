using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Services;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.Infrastructure.Pagination;
using JudgeSystem.Web.ViewModels.Practice;

using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Areas.Administration.Controllers
{
    public class PracticeController : AdministrationBaseController
    {
        private const int DefaultPage = 1;

        private readonly IPracticeService practiceService;
        private readonly ILessonService lessonService;
        private readonly ISubmissionService submissionService;
        private readonly IProblemService problemService;
        private readonly IPaginationService paginationHelper;

        public PracticeController(
            IPracticeService practiceService,
            ILessonService lessonService,
            ISubmissionService submissionService,
            IProblemService problemService,
            IPaginationService paginationHelper)
        {
            this.practiceService = practiceService;
            this.lessonService = lessonService;
            this.submissionService = submissionService;
            this.problemService = problemService;
            this.paginationHelper = paginationHelper;
        }

        public async Task<IActionResult> Submissions(string userId, int practiceId, int? problemId, int page = DefaultPage)
        {
            int baseProblemId;
            int lessonId = await practiceService.GetLessonId(practiceId);
            if (problemId.HasValue)
            {
                baseProblemId = problemId.Value;
            }
            else
            {
                baseProblemId = lessonService.GetFirstProblemId(lessonId);
            }

            var submissions = submissionService.GetUserSubmissionsByProblemIdAndPracticeId(practiceId, baseProblemId, userId, page, GlobalConstants.SubmissionsPerPage);
            string problemName = problemService.GetProblemName(baseProblemId);
            string baseUrl = GetBaseUrl(userId, practiceId);
            int submissionsCount = submissionService.GetSubmissionsCountByProblemIdAndPracticeId(baseProblemId, practiceId, userId);

            PaginationData paginationData = new PaginationData
            {
                CurrentPage = page,
                NumberOfPages = paginationHelper.CalculatePagesCount(submissionsCount, GlobalConstants.SubmissionsPerPage),
                Url = GetFullUrl(baseProblemId, baseUrl)
            };

            var model = new PracticeSubmissionsViewModel
            {
                ProblemName = problemName,
                Submissions = submissions,
                LessonId = lessonId,
                UrlPlaceholder = baseUrl + $"{GlobalConstants.QueryStringDelimiter}{GlobalConstants.ProblemIdKey}=" + "{0}",
                PaginationData = paginationData
            };

            return View(model);
        }

        private static string GetFullUrl(int baseProblemId, string baseUrl)
        {
            return baseUrl + $"{GlobalConstants.QueryStringDelimiter}{GlobalConstants.ProblemIdKey}={baseProblemId}{GlobalConstants.QueryStringDelimiter}{GlobalConstants.PageKey}" + "={0}";
        }

        private string GetBaseUrl(string userId, int practiceId)
        {
            return $"/{GlobalConstants.AdministrationArea}/Practice/{nameof(Submissions)}?practiceId={practiceId}{GlobalConstants.QueryStringDelimiter}userId={userId}";
        }
    }
}
