using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;
using System.Linq;
using System.Text;

using JudgeSystem.Common;
using JudgeSystem.Common.Exceptions;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Data.Repositories;
using JudgeSystem.Web.InputModels.Submission;
using JudgeSystem.Web.Dtos.Submission;
using JudgeSystem.Web.Dtos.ExecutedTest;
using JudgeSystem.Web.ViewModels.Submission;

using Moq;
using Xunit;

namespace JudgeSystem.Services.Data.Tests
{
    public class SubmissionServiceTests : TransientDbContextProvider
    {
        private readonly IEstimator estimator = new Estimator();

        [Fact]
        public async Task Create_WithValidData_ShouldWorkCorrect()
        {
            SubmissionService service = await CreateSubmissionService(new List<Submission>());
            string code = "using System;";
            string userId = "use_test_id";
            var submission = new SubmissionInputModel
            {
                SubmissionContent = Encoding.UTF8.GetBytes(code),
                ContestId = null,
                ProblemId = 1,
                PracticeId = 12
            };

            SubmissionDto actualSubmission = await service.Create(submission, userId);

            Assert.Equal(Encoding.UTF8.GetString(actualSubmission.Code), code);
            Assert.Equal(actualSubmission.ProblemId, submission.ProblemId);
            Assert.Contains(context.Submissions, x => x.Id == actualSubmission.Id);
        }

        [Fact]
        public async Task Create_WithNotProvidedContestIdOrPracticeId_ShouldThrowBadRequestException()
        {
            SubmissionService service = await CreateSubmissionService(new List<Submission>());
            string code = "using System;";
            string userId = "use_test_id";
            var submission = new SubmissionInputModel
            {
                SubmissionContent = Encoding.UTF8.GetBytes(code),
                ContestId = null,
                ProblemId = 1,
                PracticeId = null
            };

            await Assert.ThrowsAsync<BadRequestException>(() => service.Create(submission, userId));
        }

        [Fact]
        public async Task GetSubmissionResult_WithValidId_ShouldReturnCorrectData()
        {
            List<Submission> testData = GetDetailedTestData();
            SubmissionService service = await CreateSubmissionService(testData);

            SubmissionResult actualResult = service.GetSubmissionResult(3);
            Submission expectedResult = testData.First(x => x.Id == 3);

            Assert.Equal(expectedResult.ActualPoints, actualResult.ActualPoints);
            Assert.True(actualResult.IsCompiledSuccessfully);
            Assert.Equal(actualResult.ExecutedTests.Count, expectedResult.ExecutedTests.Count);
            foreach (ExecutedTestResult executedTest in actualResult.ExecutedTests)
            {
                Assert.Contains(expectedResult.ExecutedTests, x => x.IsCorrect == executedTest.IsCorrect 
                && x.ExecutionResultType.ToString() == executedTest.ExecutionResultType);
            }
            Assert.Equal(expectedResult.Problem.MaxPoints, actualResult.MaxPoints);
            Assert.Equal(expectedResult.SubmisionDate.ToString(GlobalConstants.StandardDateFormat, CultureInfo.InvariantCulture), 
                actualResult.SubmissionDate);
        }

        [Fact]
        public async Task GetSubmissionResult_WithInValidId_ShouldThrowEntityNotFoundException()
        {
            List<Submission> testData = GetDetailedTestData();
            SubmissionService service = await CreateSubmissionService(testData);

            Assert.Throws<EntityNotFoundException>(() => service.GetSubmissionResult(4894));
        }

        [Theory]
        [InlineData(3, "me", 1, 10, "5, 7")]
        [InlineData(2, "me", 1, 2, "8, 1")]
        [InlineData(2, "me", 2, 2, "2, 4")]
        [InlineData(2, "me", 2, 3, "4")]
        [InlineData(2, "me", 2, 5, "")]
        public async Task GetUserSubmissionsByProblemId_WithValidDataAndDifferentPages_ShouldReturnCorrectData(
            int problemId, string userId, int page, int submissionsPerPage, string expectedIds)
        {
            List<Submission> testData = PaginationTestData();
            SubmissionService service = await CreateSubmissionService(testData);

            IEnumerable<SubmissionResult> actualSubmissions = service.GetUserSubmissionsByProblemId(problemId, userId, page, submissionsPerPage);

            Assert.Equal(expectedIds, string.Join(", ", actualSubmissions.Select(x => x.Id)));
        }

        [Theory]
        [InlineData(3, "me", 2)]
        [InlineData(4, "me", 0)]
        [InlineData(2, "me123", 0)]
        public void GetProblemSubmissionsCount_ShouldReturnDifferentValues_WithDiffernertData(int problemId, 
            string userId, int expectedCount)
        {
            SubmissionService service = CreateSubmissionServiceWithMockedRepository(PaginationTestData().AsQueryable());

            int actualCount = service.GetProblemSubmissionsCount(problemId, userId);

            Assert.Equal(expectedCount, actualCount);
        }

        [Theory]
        [InlineData(2, "me", 1, 2)]
        [InlineData(3, "me", 2, 0)]
        public void GetSubmissionsCountByProblemIdAndContestId_ShouldReturnDifferentValues_WithDiffernertData(int problemId,
          string userId, int contestId, int expectedCount)
        {
            SubmissionService service = CreateSubmissionServiceWithMockedRepository(PaginationTestData().AsQueryable());

            int actualCount = service.GetSubmissionsCountByProblemIdAndContestId(problemId, contestId, userId);

            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void GetSubmissionDetails_WithValidId_ShouldReturnCorrectData()
        {
            List<Submission> testData = GetDetailedTestData();
            SubmissionService service = CreateSubmissionServiceWithMockedRepository(testData.AsQueryable());

            SubmissionViewModel actualSubmission = service.GetSubmissionDetails(1);
            Submission expectedSubmission = testData.First(x => x.Id == 1);

            Assert.Equal("using System", actualSubmission.Code);
            Assert.Equal(3, actualSubmission.ExecutedTests.Count);
            Assert.Equal(SubmissionType.PlainCode, actualSubmission.ProblemSubmissionType);
            Assert.True(actualSubmission.CompiledSucessfully);
            Assert.Empty(actualSubmission.CompilationErrors);
        }

        [Fact]
        public void GetSubmissionDetails_WithValidIdAndCompilationErrorrs_IsCompiledSuccessfullyShoulBeFalse()
        {
            List<Submission> testData = GetDetailedTestData();
            SubmissionService service = CreateSubmissionServiceWithMockedRepository(testData.AsQueryable());

            SubmissionViewModel actualSubmission = service.GetSubmissionDetails(2);
            Submission expectedSubmission = testData.First(x => x.Id == 2);

            Assert.False(actualSubmission.CompiledSucessfully);
            Assert.Equal("invalid operation", actualSubmission.CompilationErrors);
        }

        [Fact]
        public void GetSubmissionDetails_WithValidIdAndZipFileAsSubmissionType_MappedCodeShouldBeNull()
        {
            List<Submission> testData = GetDetailedTestData();
            SubmissionService service = CreateSubmissionServiceWithMockedRepository(testData.AsQueryable());

            SubmissionViewModel actualSubmission = service.GetSubmissionDetails(2);
            Submission expectedSubmission = testData.First(x => x.Id == 2);

            Assert.Empty(actualSubmission.Code);
            Assert.Equal(expectedSubmission.Problem.SubmissionType, actualSubmission.ProblemSubmissionType);
        }

        [Fact]
        public void GetSubmissionDetails_WithInValidId_ShouldReturnThrowEntityNullException()
        {
            List<Submission> testData = GetDetailedTestData();
            SubmissionService service = CreateSubmissionServiceWithMockedRepository(testData.AsQueryable());

            Assert.Throws<EntityNotFoundException>(() => service.GetSubmissionDetails(4));
        }

        [Theory]
        [InlineData(1, 2, "me", 1, 10, "8, 2")]
        [InlineData(1, 2, "me", 1, 1, "8")]
        [InlineData(1, 2, "me", 2, 1, "2")]
        [InlineData(1, 2, "me", 3, 1, "")]
        [InlineData(2, 2, "m123e", 1, 10, "")]
        public async Task GetUserSubmissionsByProblemIdAndContestId_WithValidDataAndDifferentPages_ShouldReturnCorrectData(
                   int contestId, int problemId, string userId, int page, int submissionsPerPage, string expectedIds)
        {
            List<Submission> testData = PaginationTestData();
            SubmissionService service = await CreateSubmissionService(testData);

            IEnumerable<SubmissionResult> actualSubmissions = service.GetUserSubmissionsByProblemIdAndContestId(contestId, problemId, userId, page, submissionsPerPage);

            Assert.Equal(expectedIds, string.Join(", ", actualSubmissions.Select(x => x.Id)));
        }

        [Theory]
        [InlineData(1, 100)]
        [InlineData(2, 0)]
        [InlineData(3, 50)]
        public async Task CalculateActualPoints_InDifferentCases_ShouldWorkCorrect(int id, int expectedPoints)
        {
            SubmissionService service = await CreateSubmissionService(GetDetailedTestData());

            await service.CalculateActualPoints(id);
            int actualPoints = context.Submissions.Find(id).ActualPoints;

            Assert.Equal(expectedPoints, actualPoints);
        }

        [Fact]
        public async Task CalculateActualPoints_WithInValidId_ShouldThrowEntityNotFoundException()
        {
            List<Submission> testData = GetDetailedTestData();
            SubmissionService service = await CreateSubmissionService(testData);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.CalculateActualPoints(4894));
        }

        [Fact]
        public async Task GetSubmissionCodeById_WithInValidId_ShouldThrowEntityNotFoundException()
        {
            List<Submission> testData = GetDetailedTestData();
            SubmissionService service = await CreateSubmissionService(testData);

            Assert.Throws<EntityNotFoundException>(() => service.GetSubmissionCodeById(4894));
        }

        [Fact]
        public void GetSubmissionCodeById_WithValidId_ShouldReturnCodeAsByteArray()
        {
            List<Submission> testData = GetDetailedTestData();
            SubmissionService service = CreateSubmissionServiceWithMockedRepository(testData.AsQueryable());
            int id = 2;

            byte[] code = service.GetSubmissionCodeById(id);
            byte[] expectedCode = testData.First(x => x.Id == id).Code;

            Assert.Equal(expectedCode, code);
        }

        [Fact]
        public async Task GetProblemNameBySubmissionId_WithValidId_ShouldReturnCorrectName()
        {
            SubmissionService service = await CreateSubmissionService(new List<Submission>());
            var submission = new Submission { Id = 99, Problem = new Problem { Name = "Test_Problem" } };
            await context.AddAsync(submission);
            await context.SaveChangesAsync();

            string problemName = service.GetProblemNameBySubmissionId(99);

            Assert.Equal("Test_Problem", problemName);
        }

        [Fact]
        public void GetProblemNameBySubmissionId_WithInValidId_ShouldReturnNull()
        {
            List<Submission> testData = GetDetailedTestData();
            SubmissionService service = CreateSubmissionServiceWithMockedRepository(testData.AsQueryable());

            Assert.Throws<EntityNotFoundException>(() => service.GetProblemNameBySubmissionId(999));
        }

        private async Task<SubmissionService> CreateSubmissionService(List<Submission> testData)
        {
            await context.Submissions.AddRangeAsync(testData);
            await context.SaveChangesAsync();
            IRepository<Submission> repository = new EfRepository<Submission>(context);
            var service = new SubmissionService(repository, estimator, null, null, null, null, null, null, null);
            return service;
        }

        private SubmissionService CreateSubmissionServiceWithMockedRepository(IQueryable<Submission> testData)
        {
            var reposotiryMock = new Mock<IRepository<Submission>>();
            reposotiryMock.Setup(x => x.All()).Returns(testData);
            return new SubmissionService(reposotiryMock.Object, estimator, null, null, null, null, null, null, null);
        }

        private List<Submission> GetDetailedTestData()
        {
            var user = new ApplicationUser { UserName = "Atanas" };

            return new List<Submission>
            {
                new Submission
                {
                    Id = 1,
                    Code = Encoding.UTF8.GetBytes("using System"),
                    CompilationErrors = null,
                    ExecutedTests = new List<ExecutedTest>
                    {
                        new ExecutedTest
                        {
                            IsCorrect = true,
                            TimeUsed = 5,
                            MemoryUsed = 10,
                            ExecutionResultType = TestExecutionResultType.Success,
                            Test = new Test { InputData = "Test", OutputData = "Test123", IsTrialTest = false }
                        },
                        new ExecutedTest
                        {
                            IsCorrect = true,
                            TimeUsed = 20,
                            MemoryUsed = 20,
                            ExecutionResultType = TestExecutionResultType.Success,
                            Test = new Test { InputData = "Test", OutputData = "Test123", IsTrialTest = false }
                        },
                        new ExecutedTest
                        {
                            IsCorrect = true,
                            TimeUsed = 5,
                            MemoryUsed = 10,
                            ExecutionResultType = TestExecutionResultType.Success,
                            Test = new Test { InputData = "Test", OutputData = "Test123", IsTrialTest = true }
                        }
                    },
                    Problem = new Problem { MaxPoints = 100, SubmissionType = SubmissionType.PlainCode },
                    User = user,
                    SubmisionDate = new DateTime(2019, 5, 2),
                    ProblemId = 2,
                    UserId = "test_id",
                },
                new Submission
                {
                    Id = 2,
                    Code = new byte[] {1, 12, 22, 25, 168 },
                    CompilationErrors = Encoding.UTF8.GetBytes("invalid operation"),
                    Problem = new Problem { MaxPoints = 100, SubmissionType = SubmissionType.ZipFile },
                    SubmisionDate = new DateTime(2019, 5, 2),
                    ProblemId = 1,
                    User = user,
                    UserId = "test_id",
                },
                new Submission
                {
                    Id = 3,
                    CompilationErrors = null,
                    ExecutedTests = new List<ExecutedTest>
                    {
                        new ExecutedTest
                        {
                            IsCorrect = true,
                            TimeUsed = 5,
                            MemoryUsed = 10,
                            ExecutionResultType = TestExecutionResultType.Success,
                            Test = new Test { InputData = "Test", OutputData = "Test123", IsTrialTest = true }
                        },
                        new ExecutedTest {
                            IsCorrect = false,
                            TimeUsed = 50,
                            MemoryUsed = 60,
                            ExecutionResultType = TestExecutionResultType.MemoryLimit,
                            Test = new Test { InputData = "Test", OutputData = "Test123", IsTrialTest = true }
                        },
                        new ExecutedTest
                        {
                            IsCorrect = true,
                            TimeUsed = 5,
                            MemoryUsed = 10,
                            ExecutionResultType = TestExecutionResultType.Success,
                            Test = new Test { InputData = "Test", OutputData = "Test123", IsTrialTest = false }
                        },
                        new ExecutedTest
                        {
                            IsCorrect = false,
                            TimeUsed = 51,
                            MemoryUsed = 14,
                            ExecutionResultType = TestExecutionResultType.RunTimeError,
                            Test = new Test { InputData = "Test", OutputData = "Test123", IsTrialTest = false }
                        }
                    },
                    Problem = new Problem { MaxPoints = 100, SubmissionType = SubmissionType.PlainCode },
                    ProblemId = 1,
                    User = user,
                    UserId = "test_id",
                    SubmisionDate = new DateTime(2019, 6, 2),
                 }
            };
        }

        private List<Submission> PaginationTestData()
        {
            var firstProblem = new Problem { Id = 1 };
            var secondProblem = new Problem { Id = 2 };
            var thirdProblem = new Problem { Id = 3 };

            return new List<Submission>
            {
                new Submission { Id = 1, ProblemId = 2, ContestId = null, UserId = "me", SubmisionDate = new DateTime(2019, 7, 2), Problem = secondProblem },
                new Submission { Id = 2, ProblemId = 2, ContestId = 1, UserId = "me", SubmisionDate = new DateTime(2019, 6, 2), Problem = secondProblem },
                new Submission { Id = 3, ProblemId = 1, ContestId = 2,  UserId = "me", SubmisionDate = new DateTime(2019, 8, 2), Problem = firstProblem },
                new Submission { Id = 4, ProblemId = 2, ContestId = 2, UserId = "me", SubmisionDate = new DateTime(2018, 7, 2), Problem = secondProblem },
                new Submission { Id = 5, ProblemId = 3, ContestId = null, UserId = "me", SubmisionDate = new DateTime(2019, 9, 2), Problem = thirdProblem },
                new Submission { Id = 6, ProblemId = 3, ContestId = 1, UserId = "me123", SubmisionDate = new DateTime(2019, 2, 2), Problem = thirdProblem },
                new Submission { Id = 7, ProblemId = 3, ContestId = 1, UserId = "me", SubmisionDate = new DateTime(2019, 4, 2), Problem = thirdProblem },
                new Submission { Id = 8, ProblemId = 2, ContestId = 1, UserId = "me", SubmisionDate = new DateTime(2019, 8, 2), Problem = secondProblem },
            };
        }
    }
}
