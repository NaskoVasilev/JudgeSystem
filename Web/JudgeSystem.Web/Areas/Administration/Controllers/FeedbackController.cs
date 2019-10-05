using System.Threading.Tasks;
using System.Collections.Generic;

using JudgeSystem.Common;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.Filters;
using JudgeSystem.Web.Infrastructure.Pagination;
using JudgeSystem.Web.ViewModels.Feedback;
using JudgeSystem.Web.Infrastructure.Extensions;
using JudgeSystem.Services;

using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Areas.Administration.Controllers
{
    public class FeedbackController : AdministrationBaseController
    {
        private readonly IFeedbackService feedbackService;
        private readonly IPaginationService paginationService;

        public FeedbackController(IFeedbackService feedbackService, IPaginationService paginationService)
        {
            this.feedbackService = feedbackService;
            this.paginationService = paginationService;
        }

        public IActionResult All(int page = 1)
        {
            IEnumerable<FeedbackAllViewModel> feedbacks = feedbackService.All(page, GlobalConstants.FeedbacksPerPage);
            var allFeedbacksViewModel = new AllFeedbacksViewModel()
            {
                Feedbacks = feedbacks,
                PaginationData = new PaginationData
                {
                    CurrentPage = page,
                    Url = $"/{GlobalConstants.AdministrationArea}/{nameof(FeedbackController).ToControllerName()}/{nameof(All)}?{GlobalConstants.PageKey}={{0}}",
                    NumberOfPages = paginationService.CalculatePagesCount(feedbackService.FeedbacksCount(), GlobalConstants.FeedbacksPerPage)
                }
            };

            return View(allFeedbacksViewModel);
        }

        [EndpointExceptionFilter]
        [HttpPost]
        public async Task<IActionResult> Archive(int id)
        {
            await feedbackService.Archive(id);
            return Ok(string.Format(InfoMessages.SuccessfullyDeletedMessage, "feedback"));
        }
    }
}
