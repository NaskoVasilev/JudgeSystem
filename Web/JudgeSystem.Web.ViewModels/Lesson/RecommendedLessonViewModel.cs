namespace JudgeSystem.Web.ViewModels.Lesson
{
    using JudgeSystem.Services.Mapping;
    using JudgeSystem.Data.Models;
    using JudgeSystem.Data.Models.Enums;

    public class RecommendedLessonViewModel : IMapFrom<Lesson>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public LessonType Type { get; set; }

        public int PracticeId { get; set; }

        public float Score { get; set; }
    }
}
