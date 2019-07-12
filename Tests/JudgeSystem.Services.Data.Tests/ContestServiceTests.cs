using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Repositories;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace JudgeSystem.Services.Data.Tests
{
    public class ContestServiceTests : TransientDbContextProvider
    {
        [Fact]
        public async Task Create_WithValidData_ShouldWorkCorrect()
        {
            var contest = new Contest { Name = "testContest" };
            var repository = new EfDeletableEntityRepository<Contest>(this.context);
            var contestService = new ContestService(repository, null, null);
            await contestService.Create(contest);
            Assert.True(context.Contests.Count() == 1);
            Assert.True(context.Contests.First().Name == contest.Name);
        }

        [Theory]
        [InlineData("newId", 25, true)]
        [InlineData("user_id_1", 80, true)]
        [InlineData("user_id_unknown", 2, true)]
        [InlineData("user_id_20", 2, false)]
        public async Task AddUserToContestIfNotAdded_WithCorrectData_ShouldWorkCorrect(string userId, int contestId, bool expectedResult)
        {
            context.AddRange(GetUserContestsTestData());
            await context.SaveChangesAsync();
            var repository = new EfRepository<UserContest>(this.context);
            var contestService = new ContestService(null, null, repository);
            bool result = await contestService.AddUserToContestIfNotAdded(userId, contestId);
            Assert.Equal(expectedResult, result);
        }

        private List<UserContest> GetUserContestsTestData()
        {
            return new List<UserContest>
            {
                new UserContest{ContestId = 1, UserId = "user_id_1"},
                new UserContest{ContestId = 2, UserId = "user_id_20"},
            };
        }
    }
}
