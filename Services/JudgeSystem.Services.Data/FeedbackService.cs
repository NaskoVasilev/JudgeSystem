using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Web.InputModels.Feedback;
using JudgeSystem.Web.ViewModels.Feedback;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Common.Extensions;

namespace JudgeSystem.Services.Data
{
    public class FeedbackService : IFeedbackService
    {
        private IDeletableEntityRepository<Feedback> repository;

        public FeedbackService(IDeletableEntityRepository<Feedback> repository)
        {
            this.repository = repository;
        }

        public IEnumerable<FeedbackAllViewModel> All(int page, int feedbacksPerPage)
        {
            var feedbacks =  repository.All()
                .GetPage(page, feedbacksPerPage)
                .To<FeedbackAllViewModel>()
                .ToList();
            return feedbacks;
        }

        public async Task Create(FeedbackCreateInputModel feedbackInputModel, string senderId)
        {
            Feedback feedback = feedbackInputModel.To<Feedback>();
            feedback.SenderId = senderId;
            await repository.AddAsync(feedback);
        }
    }
}
