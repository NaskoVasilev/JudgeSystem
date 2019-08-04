using System.Collections.Generic;

using JudgeSystem.Web.ViewModels.Lesson;

namespace JudgeSystem.Services.Data
{
    public interface ILessonsRecommendationService
    {
        List<RecommendedLessonViewModel> GetTopTenRecommendedLessons(string userId);
    }
}
