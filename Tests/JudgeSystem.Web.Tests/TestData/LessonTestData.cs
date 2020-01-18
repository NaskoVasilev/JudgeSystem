using System;
using System.Collections.Generic;
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

        public static IEnumerable<Lesson> GenerateLessons()
        {
            for (int i = 0; i < 100; i++)
            {
                yield return new Lesson
                {
                    Id = i + 1,
                    Name = "lesson" + i,
                    CourseId = (i % 3) + 1,
                    Type = (LessonType)((i % 3) + 1),
                    Practice = new Practice() { Id = i + 1 },
                };
            }
        }
    }
}
