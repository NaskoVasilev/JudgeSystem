using JudgeSystem.Data.Models;

namespace JudgeSystem.Web.Tests.TestData
{
    public class ResourceTestData
    {
        public static Resource GetEntity() => new Resource
        {
            Id = 1,
            FilePath = "test/file/path",
            Lesson = LessonTestData.GetEntity(),
            Name = "Conditions"
        };
    }
}
