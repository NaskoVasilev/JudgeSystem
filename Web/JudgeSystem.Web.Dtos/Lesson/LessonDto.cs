using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.Dtos.Lesson
{
    public class LessonDto : IMapFrom<Data.Models.Lesson>
    {
        public string Name { get; set; }

        public int CourseId { get; set; }

        public LessonType Type { get; set; }

        public string LessonPassword { get; set; }
    }
}
