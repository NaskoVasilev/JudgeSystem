using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Repositories;
using JudgeSystem.Web.Infrastructure.Exceptions;
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
        private readonly IEstimator estimator = new Estimator();

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
            var contestService = await CreateContestService(GetContestsTestData());
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
            var contestService = await CreateContestService(GetContestsTestData());

            var contest = context.Contests.FirstOrDefault(c => c.Id == 1);
            await contestService.DeleteContestById(1);

            Assert.True(contest.IsDeleted);
            Assert.NotNull(contest.DeletedOn);
            Assert.False(context.Contests.Count() == 2);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        [InlineData(1)]
        [InlineData(20)]
        public void GetAllConests_WithNoData_ShouldReturnEmptyCollection(int page)
        {
            var contestService = CreateContestServiceWithMockedRepository(Enumerable.Empty<Contest>().AsQueryable());

            var result = contestService.GetAllConests(page);

            Assert.Empty(result);
        }

        [Theory]
        [InlineData(1)]
        public void GetAllConests_WithValidData_ShouldReturnCorrectValues(int page)
        {
            var contestService = CreateContestServiceWithMockedRepository(Generate50ContestsWithStartDate().AsQueryable());

            var actualContests = contestService.GetAllConests(page);
            var expectedData = Enumerable.Range((page - 1) * ContestService.ContestsPerPage , page * ContestService.ContestsPerPage);

            Assert.Equal(actualContests.Select(c => c.Id), expectedData);
        }

        [Fact]
        public void GetAllConests_WithValidDataWithNotEnoughValues_ShouldReturnOnlyLastEntities()
        {
            var contestService = CreateContestServiceWithMockedRepository(Generate50ContestsWithStartDate().AsQueryable());

            int page = 50 / ContestService.ContestsPerPage + 1;
            int expectedEntities = 50 % ContestService.ContestsPerPage;
            var actualContests = contestService.GetAllConests(page);
            var expectedData = Enumerable.Range(50 - expectedEntities, expectedEntities);

            Assert.Equal(actualContests.Select(c => c.Id), expectedData);
        }

        [Fact]
        public void GetNumberOfPages_WithValidData_ShouldReturnCorrectResult()
        {
            var contestService = CreateContestServiceWithMockedRepository(Generate50ContestsWithStartDate().AsQueryable());

            var actualResult = contestService.GetNumberOfPages();

            Assert.Equal(3, actualResult);
        }

        [Fact]
        public void GetNumberOfPages_WithNoData_ShouldReturnOne()
        {
            var contestService = CreateContestServiceWithMockedRepository(Enumerable.Empty<Contest>().AsQueryable());

            var actualResult = contestService.GetNumberOfPages();

            Assert.Equal(0, actualResult);
        }

        [Fact]
        public void GetNumberOfPages_WithOneEntity_ShouldReturnOne()
        {
            var contests = new List<Contest>() { new Contest() };
            var contestService = CreateContestServiceWithMockedRepository(contests.AsQueryable());

            var actualResult = contestService.GetNumberOfPages();

            Assert.Equal(1, actualResult);
        }

        [Fact]
        public void GetById_WithInvalidId_ShouldThrowEntityNotFoundException()
        {
            var contests = new List<Contest>() { new Contest() };
            var contestService = CreateContestServiceWithMockedRepository(contests.AsQueryable());

            Assert.ThrowsAsync<EntityNotFoundException>(() => contestService.GetById<Contest>(999));
        }

        [Fact]
        public void UpdateContest_WithInvalidId_ShouldThrowEntityNotFoundException()
        {
            var contests = new List<Contest>() { new Contest() };
            var contestService = CreateContestServiceWithMockedRepository(contests.AsQueryable());

            Assert.ThrowsAsync<EntityNotFoundException>(() => contestService.UpdateContest(new ContestEditInputModel { Id = 45 }));
        }

        [Fact]
        public void DeleteContestById_WithInvalidId_ShouldThrowEntityNotFoundException()
        {
            var contests = new List<Contest>() { new Contest() };
            var contestService = CreateContestServiceWithMockedRepository(contests.AsQueryable());

            Assert.ThrowsAsync<EntityNotFoundException>(() => contestService.DeleteContestById(3));
        }

        [Fact]
        public void GetContestResultsPagesCount_WithInvalidId_ShouldThrowEntityNotFoundException()
        {
            var contests = new List<Contest>() { new Contest() };
            var contestService = CreateContestServiceWithMockedRepository(contests.AsQueryable());

            Assert.Throws<EntityNotFoundException>(() => contestService.GetContestResultsPagesCount(23));
        }

        private async Task<ContestService> CreateContestService(List<Contest> testData, IRepository<UserContest> userContestRepository)
        {
            await this.context.Contests.AddRangeAsync(testData);
            await this.context.SaveChangesAsync();
            IDeletableEntityRepository<Contest> repository = new EfDeletableEntityRepository<Contest>(this.context);
            var service = new ContestService(repository, this.estimator, userContestRepository);
            return service;
        }

        private async Task<ContestService> CreateContestService(List<Contest> testData)
        {
            return await this.CreateContestService(testData, null);
        }

        private ContestService CreateContestServiceWithMockedRepository(IQueryable<Contest> testData, IRepository<UserContest> userContestRepository)
        {
            var reposotiryMock = new Mock<IDeletableEntityRepository<Contest>>();
            reposotiryMock.Setup(x => x.All()).Returns(testData);
            return new ContestService(reposotiryMock.Object, this.estimator, userContestRepository);
        }

        private ContestService CreateContestServiceWithMockedRepository(IQueryable<Contest> testData)
        {
            return CreateContestServiceWithMockedRepository(testData, null);
        }

        private List<UserContest> GetUserContestsTestData()
        {
            return new List<UserContest>
            {
                new UserContest { ContestId = 1, UserId = "user_id_1" },
                new UserContest { ContestId = 2, UserId = "user_id_20" },
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

        private IEnumerable<Contest> Generate50ContestsWithStartDate()
        {
            for (int i = 1; i <= 50 ; i++)
            {
                yield return new Contest { Id = 50 - i, StartTime = DateTime.UtcNow.AddDays(i) };
            }
        }
    }
}
