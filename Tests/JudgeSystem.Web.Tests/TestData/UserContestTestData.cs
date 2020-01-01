using System.Collections.Generic;

using JudgeSystem.Data.Models;

namespace JudgeSystem.Web.Tests.TestData
{
    public class UserContestTestData
    {
        public static UserContest GetEntity() => new UserContest
        {
            Contest = ContestTestData.GetEntity(),
            UserId = TestApplicationUser.Id
        };

        public static IEnumerable<UserContest> GenerateUserContests()
        {
            Contest contest = ContestTestData.GetEntity();

            for (int i = 0; i < 70; i++)
            {
                string studentId = null;
                if (i % 2 == 0)
                {
                    studentId = $"student_id_{i}";
                }

                yield return new UserContest
                {
                    ContestId = contest.Id,
                    User = new ApplicationUser
                    {
                        Id = $"user_id_{i}",
                        StudentId = studentId
                    }
                };
            }
        }
    }
}
