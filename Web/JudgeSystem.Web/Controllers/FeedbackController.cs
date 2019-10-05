using System.Threading.Tasks;
using JudgeSystem.Data.Models;
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
        private readonly UserManager<ApplicationUser> userManager;

        public FeedbackController(IFeedbackService feedbackService, UserManager<ApplicationUser> userManager)
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
            return RedirectToAction("Index", "Home");
        }
    }
}
