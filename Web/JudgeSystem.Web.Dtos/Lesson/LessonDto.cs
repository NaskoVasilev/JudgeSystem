using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.Dtos.Lesson
{
    public class LessonDto : IMapFrom<Data.Models.Lesson>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CourseId { get; set; }

        public string LessonPassword { get; set; }
    }
}
