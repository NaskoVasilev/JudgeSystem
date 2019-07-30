using JudgeSystem.Web.ViewModels.Lesson;
using System.Collections.Generic;

namespace JudgeSystem.Services.Data
{
    public interface ILessonsRecommendationService
    {
        List<RecommendedLessonViewModel> GetTopTenRecommendedLessons();
    }
}
