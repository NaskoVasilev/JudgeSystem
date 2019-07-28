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
        private readonly IPaginationHelper paginationHelper;

        public PracticeController(
            IPracticeService practiceService,
            ILessonService lessonService,
            ISubmissionService submissionService,
            IProblemService problemService,
            IPaginationHelper paginationHelper)
        {
            this.practiceService = practiceService;
            this.lessonService = lessonService;
            this.submissionService = submissionService;
            this.problemService = problemService;
            this.paginationHelper = paginationHelper;
        }

        public IActionResult Submissions(string userId, int practiceId, int? problemId, int page = DefaultPage)
        {
            int baseProblemId;
            int lessonId = practiceService.GetLessonId(practiceId);
            if (problemId.HasValue)
            {
                baseProblemId = problemId.Value;
            }
            else
            {
                baseProblemId = lessonService.GetFirstProblemId(lessonId);
            }

            var submissions = submissionService.GetUserSubmissionsByProblemIdAndPracticeId(practiceId, baseProblemId, userId, page, GlobalConstants.SubmissionPerPage);
            string problemName = problemService.GetProblemName(baseProblemId);

            string baseUrl = $"/{GlobalConstants.AdministrationArea}/Practice/{nameof(submissions)}?practiceId={practiceId}&userId={userId}";

            int submissionsCount = submissionService.GetSubmissionsCountByProblemIdAndPracticeId(baseProblemId, practiceId, userId);

            PaginationData paginationData = new PaginationData
            {
                CurrentPage = page,
                NumberOfPages = paginationHelper.CalculatePagesCount(submissionsCount, GlobalConstants.SubmissionPerPage),
                Url = baseUrl + $"&problemId={baseProblemId}" + "&page={0}"
            };

            var model = new PracticeSubmissionsViewModel
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
