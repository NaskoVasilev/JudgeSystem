using System;

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
    }
}
