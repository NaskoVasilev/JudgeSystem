using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;

namespace JudgeSystem.Web.Tests.TestData
{
    public class LessonTestData
    {
        public static Lesson GetEntity() => new Lesson
        {
            Id = 1,
            Course = CourseTestData.GetEntity(),
            Name = "Loops",
            Type = LessonType.Exercise,
            Practice = PracticeTestData.GetEntity(),
        };
    }
}
