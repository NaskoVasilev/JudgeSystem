using System.Collections.Generic;
using System.Threading.Tasks;

using JudgeSystem.Web.InputModels.Feedback;
using JudgeSystem.Web.ViewModels.Feedback;

namespace JudgeSystem.Services.Data
{
    public interface IFeedbackService
    {
        Task Create(FeedbackCreateInputModel feedback, string senderId);

        IEnumerable<FeedbackAllViewModel> All(int page, int feedbacksPerPage);
    }
}
