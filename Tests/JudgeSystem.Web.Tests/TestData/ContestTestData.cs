using System;
using System.Collections.Generic;

using JudgeSystem.Data.Models;

namespace JudgeSystem.Web.Tests.TestData
{
    public class ContestTestData
    {
        public static Contest GetEntity() => new Contest
        {
            EndTime = DateTime.Now.AddDays(2),
            StartTime = DateTime.Now.AddDays(-1),
            Id = 1,
            Lesson = LessonTestData.GetEntity(),
            Name = "Loops - Competition"
        };

        public static IEnumerable<Contest> GetContests()
        {
            Lesson lesson = LessonTestData.GetEntity();

            return new List<Contest>
            {
                new Contest
                {
                    Id = 1,
                    EndTime = DateTime.Now.AddDays(2),
                    StartTime = DateTime.Now.AddDays(-1),
                    Name = "Loops - Competition",
                    Lesson = lesson
                },
                new Contest
                {
                    Id = 2,
                    EndTime = DateTime.Now.AddDays(5),
                    StartTime = DateTime.Now.AddDays(3),
                    Name = "Variables - Exam",
                    Lesson = lesson
                },
                new Contest
                {
                    Id = 3,
                    EndTime = DateTime.Now.AddDays(-5),
                    StartTime = DateTime.Now.AddDays(-4),
                    Name = "Loops - Competition - Old",
                    Lesson = lesson
                }
            };
        }

        public static IEnumerable<Contest> GenerateContests()
        {
            for (int i = 0; i < 36; i++)
            {
                int addDays = i;
                if (i % 2 == 0)
                {
                    addDays = i - 1;
                }

                yield return new Contest
                {
                    EndTime = DateTime.Now.AddDays(addDays),
                    StartTime = DateTime.Now.AddDays(addDays),
                    Id = i + 1,
                    Name = $"Contest - {i}"
                };
            }
        }
    }
}
