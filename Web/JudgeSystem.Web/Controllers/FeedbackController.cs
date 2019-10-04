using System.Threading.Tasks;

using JudgeSystem.Services.Data;
using JudgeSystem.Web.InputModels.Feedback;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Controllers
{
    [Authorize]
    public class FeedbackController : BaseController
    {
        private readonly IFeedbackService feedbackService;
        private readonly UserManager<IdentityUser> userManager;

        public FeedbackController(IFeedbackService feedbackService, UserManager<IdentityUser> userManager)
        {
            this.feedbackService = feedbackService;
            this.userManager = userManager;
        }

        public IActionResult Send() => View();

        [HttpPost]
        public async Task<IActionResult> Send(FeedbackCreateInputModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            await feedbackService.Create(model, userManager.GetUserId(User));
            return RedirectToAction("Home", "Index");
        }
    }
}
