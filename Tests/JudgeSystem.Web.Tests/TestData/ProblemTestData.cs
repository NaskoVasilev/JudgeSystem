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
            Name = "Sum tow numbers",
            Lesson = LessonTestData.GetEntity(),
            MaxPoints = 100,
            SubmissionType = SubmissionType.PlainCode
        };
    }
}
