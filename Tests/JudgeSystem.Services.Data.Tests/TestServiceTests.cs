using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Common.Exceptions;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Repositories;
using JudgeSystem.Web.Dtos.Test;
using JudgeSystem.Web.InputModels.Test;
using JudgeSystem.Web.ViewModels.Test;

using Moq;
using Xunit;

namespace JudgeSystem.Services.Data.Tests
{
    public class TestServiceTests : TransientDbContextProvider
    {
        [Fact]
        public async Task Add_WithValidData_ShouldWorkCorrect()
        {
            TestService service = await CreateTestService(new List<Test>());
            var test = new TestInputModel { InputData = "123", OutputData = "321", IsTrialTest = false, ProblemId = 5 };

            TestDto createdTest = await service.Add(test);
            Test actualTest = context.Tests.FirstOrDefault(x => x.Id == createdTest.Id);

            Assert.NotNull(actualTest);
            Assert.Equal(test.InputData, actualTest.InputData);
            Assert.Equal(test.OutputData, actualTest.OutputData);
            Assert.False(actualTest.IsTrialTest);
            Assert.Equal(test.ProblemId, actualTest.ProblemId);
        }

        [Fact]
        public async Task Delete_WithValidData_ShouldWorkCorrect()
        {
            var executedTests = new List<ExecutedTest>
            {
                new ExecutedTest { Id = 5 },
                new ExecutedTest { Id = 3 }
            };
            TestService service = await CreateTestService(new List<Test>(), executedTests);
            var test = new Test
            {
                Id = 1,
                ExecutedTests = executedTests
            };
            await context.AddAsync(test);
            await context.SaveChangesAsync();

            await service.Delete(1);

            Assert.Empty(context.ExecutedTests);
            Assert.Empty(context.Tests);
        }

        [Fact]
        public async Task Delete_WithNonExistingStudent_ShouldThrowError()
        {
            TestService service = await CreateTestService(new List<Test>());

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.Delete(999));
        }

        [Fact]
        public async Task GetById_WithValidId_ShouldReturnCorrectData()
        {
            List<Test> testData = GetTestData();
            TestService service = await CreateTestService(testData);

            TestDto actualData = await service.GetById<TestDto>(3);
            Test expectedData = testData.First(x => x.Id == 3);

            Assert.Equal(expectedData.InputData, actualData.InputData);
            Assert.Equal(expectedData.OutputData, actualData.OutputData);
            Assert.Equal(expectedData.IsTrialTest, actualData.IsTrialTest);
        }

        [Fact]
        public async Task GetById_WithInValidId_ShouldReturnNull()
        {
            List<Test> testData = GetTestData();
            TestService service = await CreateTestService(testData);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetById<TestEditInputModel>(4875));
        }

        [Theory]
        [InlineData(5, "1, 2, 3")]
        [InlineData(45, "")]
        public async Task GetTestsByProblemId_WithDifferntData_ShouldReturnDifferentResults(int problemId, string expectedIds)
        {
            TestService service = await CreateTestService(GetTestData());

            IEnumerable<TestDataDto> results = service.GetTestsByProblemId(problemId);

            Assert.Equal(expectedIds, string.Join(", ", results.Select(x => x.Id)));
        }

        [Theory]
        [InlineData(5, "1, 3, 2")]
        [InlineData(45, "")]
        public async Task GetTestsByProblemIdOrderedByIsTrialDescending_WithDifferntData_ShouldReturnDifferentResults(int problemId, string expectedIds)
        {
            TestService service = await CreateTestService(GetTestData());

            IEnumerable<TestViewModel> results = service.GetTestsByProblemIdOrderedByIsTrialDescending(problemId);

            Assert.Equal(expectedIds, string.Join(", ", results.Select(x => x.Id)));
        }

        [Fact]
        public async Task Update_WithValidData_ShouldWorkCorrect()
        {
            List<Test> testData = GetTestData();
            TestService service = await CreateTestService(testData);
            var testInputModel = new TestEditInputModel
            {
                Id = 3,
                InputData = "test123",
                OutputData = "321tset0",
            };

            await service.Update(testInputModel);
            Test actualTest = context.Tests.First(x => x.Id == testInputModel.Id);

            Assert.Equal(actualTest.InputData, testInputModel.InputData);
            Assert.Equal(actualTest.OutputData, testInputModel.OutputData);
        }

        [Fact]
        public async Task Update_WithNonExistingLesson_ShouldThrowArgumentEException()
        {
            TestService service = await CreateTestService(GetTestData());

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.Update(new TestEditInputModel() { Id = 99 }));
        }

        private async Task<TestService> CreateTestService(List<Test> tests, List<ExecutedTest> executedTests = null)
        {
            if (executedTests == null)
            {
                executedTests = new List<ExecutedTest>();
            }

            await context.Tests.AddRangeAsync(tests);
            await context.ExecutedTests.AddRangeAsync(executedTests);
            await context.SaveChangesAsync();
            IRepository<Test> testRepository = new EfRepository<Test>(context);
            IRepository<ExecutedTest> executedTestRepository = new EfRepository<ExecutedTest>(context);
            IExecutedTestService executedTestService = new ExecutedTestService(executedTestRepository);
            var service = new TestService(testRepository, executedTestService);
            return service;
        }

        private TestService CreateTestServiceWithMockedRepository(IQueryable<Test> tests, IQueryable<ExecutedTest> executedTests)
        {
            var testReposotoryMock = new Mock<IRepository<Test>>();
            testReposotoryMock.Setup(x => x.All()).Returns(tests);

            var executedTestReposotoryMock = new Mock<IRepository<ExecutedTest>>();
            executedTestReposotoryMock.Setup(x => x.All()).Returns(executedTests);

            var executedTestService = new ExecutedTestService(executedTestReposotoryMock.Object);

            return new TestService(testReposotoryMock.Object, executedTestService);
        }

        private List<Test> GetTestData()
        {
            var tests = new List<Test>()
            {
                new Test { Id = 1, InputData = "123", OutputData = "321", IsTrialTest= true, ProblemId = 5 },
                new Test { Id = 2, InputData = "abc", OutputData = "bca", IsTrialTest= false, ProblemId = 5 },
                new Test { Id = 3, InputData = "1a1", OutputData = "1a1", IsTrialTest= true, ProblemId = 5 },
                new Test { Id = 4, InputData = "444", OutputData = "888", IsTrialTest= false, ProblemId = 4 },
                new Test { Id = 5, InputData = "333", OutputData = "666", IsTrialTest= false, ProblemId = 4 },
            };
            return tests;
        }
    }
}
