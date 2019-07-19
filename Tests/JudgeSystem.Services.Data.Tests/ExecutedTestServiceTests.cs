using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Repositories;
using JudgeSystem.Services.Data.Tests.ClassFixtures;
using Xunit;

namespace JudgeSystem.Services.Data.Tests
{
    public class ExecutedTestServiceTests : SingletonDbContextProvider
    {
        public ExecutedTestServiceTests(InMemoryDatabaseFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Create_WithValidData_ShouldWorkCorrect()
        {
            IRepository<ExecutedTest> repository = new EfRepository<ExecutedTest>(this.Context);
            ExecutedTestService service = new ExecutedTestService(repository);

            int initialCount = this.Context.ExecutedTests.Count();
            await service.Create(new ExecutedTest());

            Assert.Equal(initialCount + 1, this.Context.ExecutedTests.Count());
        }

        [Fact]
        public async Task DeleteExecutedTestsByTestId_WithCorrectTestId_ShouldWorkCorrect()
        {
            await this.Context.ExecutedTests.AddRangeAsync(GetTestData());
            await this.Context.SaveChangesAsync();
            IRepository<ExecutedTest> repository = new EfRepository<ExecutedTest>(this.Context);
            ExecutedTestService service = new ExecutedTestService(repository);

            await service.DeleteExecutedTestsByTestId(4);

            Assert.True(this.Context.ExecutedTests.Count() > 0);
            Assert.True(this.Context.ExecutedTests.Count(x => x.TestId == 4) == 0);
        }

        [Fact]
        public async Task DeleteExecutedTestsByTestId_WithIncorrectTestId_ShouldDoNothing()
        {
            var testData = GetTestData();
            await this.Context.ExecutedTests.AddRangeAsync(testData);
            await this.Context.SaveChangesAsync();
            IRepository<ExecutedTest> repository = new EfRepository<ExecutedTest>(this.Context);
            ExecutedTestService service = new ExecutedTestService(repository);

            await service.DeleteExecutedTestsByTestId(5);

            Assert.True(this.Context.ExecutedTests.Count() == testData.Count());
        }

        private List<ExecutedTest> GetTestData()
        {
            return new List<ExecutedTest>()
            {
                new ExecutedTest(){TestId = 4},
                new ExecutedTest(){TestId = 4},
                new ExecutedTest(){TestId = 3},
                new ExecutedTest(){TestId = 2},
                new ExecutedTest(){TestId = 1},
                new ExecutedTest(){TestId = 4},
                new ExecutedTest(){TestId = 3},
            };
        }
    }
}
