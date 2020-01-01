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

        public static IEnumerable<Lesson> GetLessons()
        {
            Course defaultCourse = CourseTestData.GetEntity();
            var extraCourse = new Course
            {
                Name = "OOP",
                Id = 2
            };

            return new List<Lesson>
            {
                new Lesson
                {
                    Id = 1,
                    Course = defaultCourse,
                    Name = "Loops",
                    Type = LessonType.Exercise,
                    Practice = new Practice { Id = 2 }
                },
                new Lesson
                {
                    Id = 2,
                    Course = defaultCourse,
                    Name = "Conditions",
                    Type = LessonType.Exam,
                    Practice = new Practice{ Id = 3 }
                },
                new Lesson
                {
                    Id = 3,
                    Course = defaultCourse,
                    Name = "Loops",
                    Type = LessonType.Exam,
                    Practice = new Practice{ Id = 4 }
                },
                new Lesson
                {
                    Id = 4,
                    Course = extraCourse,
                    Name = "Defining Classes",
                    Type = LessonType.Exercise,
                    Practice = new Practice{ Id = 5 }
                }
            };
        }
    }
}
