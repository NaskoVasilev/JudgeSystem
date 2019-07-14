using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Repositories;
using JudgeSystem.Web.InputModels.Contest;
using Moq;
using System;
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

        [Fact]
        public async Task UpdateContest_ShouldWorkCorrectWithCorrectData()
        {
            context.Contests.AddRange(GetContestsTestData());
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<Contest>(this.context);
            var contestService = new ContestService(repository, null, null);
            var inputModel = new ContestEditInputModel { Id = 1, Name = "editedcontest", StartTime = new DateTime(2019, 09, 20), EndTime = new DateTime(2019, 08, 20) };
            await contestService.UpdateContest(inputModel);
            var actualResult = context.Contests.First(x => x.Id == 1);
            Assert.Equal(inputModel.Name, actualResult.Name);
            Assert.Equal(inputModel.StartTime, actualResult.StartTime);
            Assert.Equal(inputModel.EndTime, actualResult.EndTime);
        }

        [Fact]
        public async Task DeleteContestById_ShouldWorkCorrectWithValidData()
        {
            context.Contests.AddRange(GetContestsTestData());
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<Contest>(this.context);
            var contestService = new ContestService(repository, null, null);
            var contest = context.Contests.FirstOrDefault(c => c.Id == 1);
            await contestService.DeleteContestById(1);
            Assert.True(contest.IsDeleted);
            Assert.NotNull(contest.DeletedOn);
            Assert.False(context.Contests.Count() == 2);
        }



        private List<UserContest> GetUserContestsTestData()
        {
            return new List<UserContest>
            {
                new UserContest{ContestId = 1, UserId = "user_id_1"},
                new UserContest{ContestId = 2, UserId = "user_id_20"},
            };
        }

        private List<Contest> GetContestsTestData()
        {
            return new List<Contest>
            {
                new Contest{Id = 1, Name = "contest1", EndTime = new DateTime(2019, 12, 20), StartTime = new DateTime(2019, 07, 05)},
                new Contest{Id = 2, Name = "contest2", EndTime = new DateTime(2019, 05, 20), StartTime = new DateTime(2019, 04, 05)},
            };
        }
    }
}
