using System.Collections.Generic;
using System.Security.Claims;

using JudgeSystem.Data.Models;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.ViewModels.Lesson;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
            string userId = userManager.GetUserId(User as ClaimsPrincipal);
            List<RecommendedLessonViewModel> lessons = lessonsRecommendationService.GetTopTenRecommendedLessons(userId);
            return View(lessons);
        }       
    }
}
