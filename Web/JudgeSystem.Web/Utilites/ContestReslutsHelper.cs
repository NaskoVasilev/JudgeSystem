using JudgeSystem.Common;
using JudgeSystem.Services;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.Infrastructure.Pagination;
using JudgeSystem.Web.ViewModels.Contest;

namespace JudgeSystem.Web.Utilites
{
    public class ContestReslutsHelper
    {
        private readonly ILessonService lessonService;
        private readonly IContestService contestService;
        private readonly ISubmissionService submissionService;
        private readonly IProblemService problemService;
        private readonly IPaginationService paginationService;

        public ContestReslutsHelper(
            ILessonService lessonService,
            IContestService contestService,
            ISubmissionService submissionService,
            IProblemService problemService,
            IPaginationService paginationService)
        {
            this.lessonService = lessonService;
            this.contestService = contestService;
            this.submissionService = submissionService;
            this.problemService = problemService;
            this.paginationService = paginationService;
        }

        public ContestSubmissionsViewModel GetContestSubmissions(int contestId, string userId, int? problemId, int page, string baseUrl)
        {
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

            int submissionsCount = submissionService.GetSubmissionsCountByProblemIdAndContestId(baseProblemId, contestId, userId);

            PaginationData paginationData = new PaginationData
            {
                CurrentPage = page,
                NumberOfPages = paginationService.CalculatePagesCount(submissionsCount, GlobalConstants.SubmissionPerPage),
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

            return model;
        }
    }
}
