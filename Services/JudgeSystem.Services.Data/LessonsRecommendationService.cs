namespace JudgeSystem.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using JudgeSystem.Data.Common.Repositories;
    using JudgeSystem.Data.Models;
    using JudgeSystem.Services.Mapping;
    using JudgeSystem.Web.Dtos.ML;
    using JudgeSystem.Web.ViewModels.Lesson;

    using Microsoft.Extensions.ML;

    public class LessonsRecommendationService : ILessonsRecommendationService
    {
        private const int CountOfRecommendedLessons = 10;
        private readonly PredictionEnginePool<UserLesson, UserLessonScore> predictionEngine;
        private readonly IDeletableEntityRepository<Lesson> lessonRepository;
        private readonly IRepository<UserPractice> userPracticeRepository;

        public LessonsRecommendationService(
            IDeletableEntityRepository<Lesson> lessonRepository, 
            IRepository<UserPractice> userPracticeRepository,
            PredictionEnginePool<UserLesson, UserLessonScore> predictionEngine)
        {
            this.lessonRepository = lessonRepository;
            this.userPracticeRepository = userPracticeRepository;
            this.predictionEngine = predictionEngine;
        }

        public List<RecommendedLessonViewModel> GetTopTenRecommendedLessons(string userId)
        {
            var userPracticeIds = userPracticeRepository.All()
                .Where(x => x.UserId == userId)
                .Select(x => x.PracticeId);
            var userPraciceIdsSet = new HashSet<int>(userPracticeIds);

            var lessons = this.lessonRepository.All()
                .Where(l => !userPraciceIdsSet.Contains(l.Practice.Id))
                .To<RecommendedLessonViewModel>()
                .ToList();

            foreach (var lesson in lessons)
            {
                var userLesson = new UserLesson { LessonId = lesson.Id, UserId = userId };
                var prediction = predictionEngine.Predict(userLesson);
                lesson.Score = prediction.Score;
            }

            return lessons
                .OrderByDescending(x => x.Score)
                .Take(CountOfRecommendedLessons)
                .ToList();
        }
    }
}
