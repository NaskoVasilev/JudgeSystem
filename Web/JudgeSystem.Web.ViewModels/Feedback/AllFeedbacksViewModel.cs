using System.Collections.Generic;

using JudgeSystem.Web.Infrastructure.Pagination;

namespace JudgeSystem.Web.ViewModels.Feedback
{
    public class AllFeedbacksViewModel
    {
        public IEnumerable<FeedbackAllViewModel> Feedbacks { get; set; }

        public PaginationData PaginationData { get; set; }
    }
}
