using System;
using System.Collections.Generic;
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

        public static IEnumerable<Submission> GenerateSubmissions()
        {
            Contest contest = ContestTestData.GetEntity();
            Problem problem = ProblemTestData.GetEntity();
            byte[] code = Encoding.UTF8.GetBytes(Code);
            var test = new Test { Id = 1 };

            for (int i = 0; i < 40; i++)
            {
                string userId = TestApplicationUser.Id;
                if (i % 2 == 0)
                {
                    userId = TestConstnts.OtherUserId;
                }

                yield return new Submission
                {
                    Id = i + 1,
                    ActualPoints = i,
                    Code = code,
                    ContestId = contest.Id,
                    ProblemId = problem.Id,
                    UserId = userId,
                    SubmisionDate = DateTime.Now.AddDays(-i),
                    ExecutedTests = new List<ExecutedTest>
                    {
                        new ExecutedTest
                        {
                            Id = i + 1,
                            TestId = test.Id
                        }
                    }
                };
            }
        }
    }
}
