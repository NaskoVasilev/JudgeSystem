using JudgeSystem.Data.Models;
using JudgeSystem.Services.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JudgeSystem.Web.Components
{
    [ViewComponent(Name = "RecommendedLessons")]
    public class RecommendedLessonsViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILessonsRecommendationService lessonsRecommendationService;

        public RecommendedLessonsViewComponent(
            UserManager<ApplicationUser> userManager,
            ILessonsRecommendationService lessonsRecommendationService)
        {
            this.userManager = userManager;
            this.lessonsRecommendationService = lessonsRecommendationService;
        }

        public IViewComponentResult Invoke()
        {
            string userId = userManager.GetUserId(this.User as ClaimsPrincipal);
            var lessons = lessonsRecommendationService.GetTopTenRecommendedLessons(userId);
            return View(lessons);
        }       
    }
}
