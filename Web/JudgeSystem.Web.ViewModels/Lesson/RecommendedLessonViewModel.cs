using JudgeSystem.Services.Mapping;
using JudgeSystem.Data.Models.Enums;

namespace JudgeSystem.Web.ViewModels.Lesson
{
    public class RecommendedLessonViewModel : IMapFrom<Data.Models.Lesson>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public LessonType Type { get; set; }

        public int PracticeId { get; set; }

        public float Score { get; set; }
    }
}
