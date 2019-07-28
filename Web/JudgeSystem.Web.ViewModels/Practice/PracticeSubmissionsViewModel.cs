using JudgeSystem.Web.Dtos.Submission;
using JudgeSystem.Web.Infrastructure.Pagination;
using System.Collections.Generic;

namespace JudgeSystem.Web.ViewModels.Practice
{
    public class PracticeSubmissionsViewModel
    {
        public IEnumerable<SubmissionResult> Submissions { get; set; }

        public string ProblemName { get; set; }

        public int LessonId { get; set; }

        public string UrlPlaceholder { get; set; }

        public PaginationData PaginationData { get; set; }
    }
}
