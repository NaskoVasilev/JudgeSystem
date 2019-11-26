using System.Text;

using JudgeSystem.Data.Models;

namespace JudgeSystem.Web.Tests.TestData
{
    public class SubmissionTestData
    {
        public const string Code = "using System;";

        public static Submission GetEntity() => new Submission
        {
            Id = 1,
            ActualPoints = 70,
            Code = Encoding.UTF8.GetBytes(Code),
            Contest = ContestTestData.GetEntity(),
            Problem = ProblemTestData.GetEntity(),
            UserId = TestApplicationUser.Id
        };
    }
}
