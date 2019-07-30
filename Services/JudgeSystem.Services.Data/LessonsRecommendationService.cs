namespace JudgeSystem.Services.Data
{
    using System.Collections.Generic;

    using JudgeSystem.Data.Common.Repositories;
    using JudgeSystem.Data.Models;
    using JudgeSystem.Web.Dtos.ML;
    using JudgeSystem.Web.ViewModels.Lesson;

    using Microsoft.Extensions.ML;

    public class LessonsRecommendationService : ILessonsRecommendationService
    {
        private readonly PredictionEnginePool<UserLesson, UserLessonScore> predictionEngine;
        private readonly IDeletableEntityRepository<Lesson> lessonRepository;

        public LessonsRecommendationService(
            IDeletableEntityRepository<Lesson> lessonRepository, 
            PredictionEnginePool<UserLesson, UserLessonScore> predictionEngine)
        {
            this.lessonRepository = lessonRepository;
            this.predictionEngine = predictionEngine; ;
        }

        public List<RecommendedLessonViewModel> GetTopTenRecommendedLessons()
        {
            throw new System.NotImplementedException();
        }
    }
}
