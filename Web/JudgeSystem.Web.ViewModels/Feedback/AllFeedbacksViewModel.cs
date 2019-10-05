using JudgeSystem.Web.Infrastructure.Pagination;
using System.Collections.Generic;

namespace JudgeSystem.Web.ViewModels.Feedback
{
    public class AllFeedbacksViewModel
    {
        public IEnumerable<FeedbackAllViewModel> Feedbacks { get; set; }

        public PaginationData PaginationData { get; set; }
    }
}
