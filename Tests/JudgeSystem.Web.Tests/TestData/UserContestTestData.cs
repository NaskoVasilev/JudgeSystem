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
    }
}
