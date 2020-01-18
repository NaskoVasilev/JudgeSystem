using JudgeSystem.Data.Models;
using System.Collections.Generic;

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

        public static IEnumerable<Resource> GenerateResources()
        {
            for (int i = 0; i < 10; i++)
            {
                yield return new Resource
                {
                    Id = i + 1,
                    FilePath = "test file path",
                    Name = $"resource#{i}",
                    LessonId = (i % 2) + 1
                };
            }
        }
    }
}
