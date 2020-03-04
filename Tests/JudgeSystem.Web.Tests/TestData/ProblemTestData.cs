using System.Collections.Generic;

using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;

namespace JudgeSystem.Web.Tests.TestData
{
    public class ProblemTestData
    {
        public static Problem GetEntity() => new Problem
        {
            Id = 1,
            AllowedMemoryInMegaBytes = 10,
            AllowedTimeInMilliseconds = 500,
            Name = "Sum two numbers",
            Lesson = LessonTestData.GetEntity(),
            MaxPoints = 100,
            SubmissionType = SubmissionType.PlainCode,
            TimeIntervalBetweenSubmissionInSeconds = 20
        };

        internal static IEnumerable<Problem> GetEntities()
        {
            Lesson lesson = LessonTestData.GetEntity();

            return new List<Problem>
            {
                new Problem
                {
                    LessonId = lesson.Id,
                    Name = "Sum in matrix",
                },
                new Problem
                {
                    LessonId = lesson.Id,
                    Name = "Matrix diagonals"
                },
                new Problem
                {
                    LessonId = 100,
                    Name = "Reflection"
                }
            };
        }
    }
}
