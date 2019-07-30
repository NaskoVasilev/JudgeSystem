namespace JudgeSystem.Web.ViewModels.Lesson
{
    using JudgeSystem.Services.Mapping;
    using JudgeSystem.Data.Models;

    public class RecommendedLessonViewModel : IMapFrom<Lesson>
    {
        public int Id { get; set; }

        public int Name { get; set; }

        public int PracticeId { get; set; }

        public float Score { get; set; }
    }
}
