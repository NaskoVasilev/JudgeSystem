using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Common.Exceptions;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Repositories;
using JudgeSystem.Web.InputModels.Contest;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Common;
using JudgeSystem.Web.Dtos.Submission;
using JudgeSystem.Web.ViewModels.Contest;
using JudgeSystem.Web.ViewModels.Problem;

using Xunit;
using Moq;

namespace JudgeSystem.Services.Data.Tests
{
    public class ContestServiceTests : TransientDbContextProvider
    {
        private readonly IPaginationService paginationService = new PaginationService();

        [Fact]
        public async Task Create_WithValidData_ShouldWorkCorrect()
        {
            var contest = new ContestCreateInputModel { Name = "testContest" };
            var repository = new EfDeletableEntityRepository<Contest>(context);
            var contestService = new ContestService(repository, null, null, null, null, null);

            await contestService.Create(contest);

            Assert.True(context.Contests.Count() == 1);
            Assert.True(context.Contests.First().Name == contest.Name);
        }

        [Theory]
        [InlineData("newId", 25, true)]
        [InlineData("user_id_1", 80, true)]
        [InlineData("user_id_unknown", 2, true)]
        [InlineData("user_id_20", 2, false)]
        public async Task AddUserToContestIfNotAdded_WithDifferenrtData_ShouldWorkCorrect(string userId, int contestId, bool expectedResult)
        {
            context.AddRange(GetUserContestsTestData());
            await context.SaveChangesAsync();
            var repository = new EfRepository<UserContest>(context);
            var contestService = new ContestService(null, repository, null, null, null, null);

            bool result = await contestService.AddUserToContestIfNotAdded(userId, contestId);

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task UpdateContest_WithCorrectData_ShouldWorkCorrect()
        {
            ContestService contestService = await CreateContestService(GetContestsTestData());
            var inputModel = new ContestEditInputModel { Id = 1, Name = "editedcontest", StartTime = new DateTime(2019, 09, 20), EndTime = new DateTime(2019, 08, 20) };

            await contestService.Update(inputModel);
            Contest actualResult = context.Contests.First(x => x.Id == 1);

            Assert.Equal(inputModel.Name, actualResult.Name);
            Assert.Equal(inputModel.StartTime, actualResult.StartTime);
            Assert.Equal(inputModel.EndTime, actualResult.EndTime);
        }

        [Fact]
        public async Task DeleteContestById_WithValidData_ShouldWorkCorrectWithValidData()
        {
            ContestService contestService = await CreateContestService(GetContestsTestData());

            Contest contest = context.Contests.FirstOrDefault(c => c.Id == 1);
            await contestService.Delete(1);

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
            ContestService contestService = CreateContestServiceWithMockedRepository(Enumerable.Empty<Contest>().AsQueryable());

            IEnumerable<ContestViewModel> result = contestService.GetAllConests(page);

            Assert.Empty(result);
        }

        [Theory]
        [InlineData(1)]
        public void GetAllConests_WithValidData_ShouldReturnCorrectValues(int page)
        {
            ContestService contestService = CreateContestServiceWithMockedRepository(Generate50ContestsWithStartDate().AsQueryable());

            IEnumerable<ContestViewModel> actualContests = contestService.GetAllConests(page);
            IEnumerable<int> expectedData = Enumerable.Range((page - 1) * GlobalConstants.ContestsPerPage, page * GlobalConstants.ContestsPerPage);

            Assert.Equal(actualContests.Select(c => c.Id), expectedData);
        }

        [Fact]
        public void GetAllConests_WithValidDataWithNotEnoughValues_ShouldReturnOnlyLastEntities()
        {
            ContestService contestService = CreateContestServiceWithMockedRepository(Generate50ContestsWithStartDate().AsQueryable());

            int page = (50 / GlobalConstants.ContestsPerPage) + 1;
            int expectedEntities = 50 % GlobalConstants.ContestsPerPage;
            IEnumerable<ContestViewModel> actualContests = contestService.GetAllConests(page);
            IEnumerable<int> expectedData = Enumerable.Range(50 - expectedEntities, expectedEntities);

            Assert.Equal(actualContests.Select(c => c.Id), expectedData);
        }

        [Fact]
        public void GetNumberOfPages_WithValidData_ShouldReturnCorrectResult()
        {
            ContestService contestService = CreateContestServiceWithMockedRepository(Generate50ContestsWithStartDate().AsQueryable());

            int actualResult = contestService.GetNumberOfPages();

            Assert.Equal(5, actualResult);
        }

        [Fact]
        public void GetNumberOfPages_WithNoData_ShouldReturnOne()
        {
            ContestService contestService = CreateContestServiceWithMockedRepository(Enumerable.Empty<Contest>().AsQueryable());

            int actualResult = contestService.GetNumberOfPages();

            Assert.Equal(0, actualResult);
        }

        [Fact]
        public void GetNumberOfPages_WithOneEntity_ShouldReturnOne()
        {
            var contests = new List<Contest>() { new Contest() };
            ContestService contestService = CreateContestServiceWithMockedRepository(contests.AsQueryable());

            int actualResult = contestService.GetNumberOfPages();

            Assert.Equal(1, actualResult);
        }

        [Fact]
        public void GetById_WithInvalidId_ShouldThrowEntityNotFoundException()
        {
            var contests = new List<Contest>() { new Contest() };
            ContestService contestService = CreateContestServiceWithMockedRepository(contests.AsQueryable());

            Assert.ThrowsAsync<EntityNotFoundException>(() => contestService.GetById<Contest>(999));
        }

        [Fact]
        public void UpdateContest_WithInvalidId_ShouldThrowEntityNotFoundException()
        {
            var contests = new List<Contest>() { new Contest() };
            ContestService contestService = CreateContestServiceWithMockedRepository(contests.AsQueryable());

            Assert.ThrowsAsync<EntityNotFoundException>(() => contestService.Update(new ContestEditInputModel { Id = 45 }));
        }

        [Fact]
        public void DeleteContestById_WithInvalidId_ShouldThrowEntityNotFoundException()
        {
            var contests = new List<Contest>() { new Contest() };
            ContestService contestService = CreateContestServiceWithMockedRepository(contests.AsQueryable());

            Assert.ThrowsAsync<EntityNotFoundException>(() => contestService.Delete(3));
        }

        [Fact]
        public void GetContestResultsPagesCount_WithInvalidId_ShouldThrowEntityNotFoundException()
        {
            var contests = new List<Contest>() { new Contest() };
            ContestService contestService = CreateContestServiceWithMockedRepository(contests.AsQueryable());

            Assert.Throws<EntityNotFoundException>(() => contestService.GetContestResultsPagesCount(23));
        }

        [Fact]
        public async Task GetActiveContests_WithDifferentData_ShouldReturnsOnlyActiveContests()
        {
            ContestService service = await CreateContestService(GetTestData());

            IEnumerable<ActiveContestViewModel> actualData = service.GetActiveContests();

            Assert.Equal(new List<int>() { 1, 3 }, actualData.Select(x => x.Id));
        }

        [Fact]
        public async Task GetPreviousContests_WithDifferentData_ShouldReturnsOnlyPassedContests()
        {
            ContestService service = await CreateContestService(GetTestData());

            IEnumerable<PreviousContestViewModel> actualData = service.GetPreviousContests(3);

            Assert.Equal(new List<string>() { "compete5" }, actualData.Select(x => x.Name));
        }

        [Fact]
        public async Task GetActiveAndFollowingContests_WithDifferentData_ShouldReturnsOnlyActiveAndFollowingContests()
        {
            ContestService service = await CreateContestService(GetTestData());

            IEnumerable<ContestBreifInfoViewModel> actualData = service.GetActiveAndFollowingContests();

            Assert.Equal(new List<string>() { "compete1", "compete3", "compete4" }, actualData.Select(x => x.Name));
        }

        [Fact]
        public async Task GetContestReults_WithValidContestId_ShouldWorkCorrect()
        {
            List<Contest> testData = GetContestReultsTestData();
            ContestService service = await CreateContestService(testData);

            ContestAllResultsViewModel actualContest = service.GetContestReults(11, 1);
            Contest expectedContest = testData.First();
            var expectedProblems = expectedContest.Lesson.Problems.OrderBy(x => x.CreatedOn).ToList();

            Assert.Equal(expectedContest.Name, actualContest.Name);
            Assert.Equal(1, actualContest.NumberOfPages);
            Assert.Equal(1, actualContest.CurrentPage);
            Assert.Equal(expectedProblems.Count, actualContest.Problems.Count);
            for (int i = 0; i < actualContest.Problems.Count; i++)
            {
                ContestProblemViewModel actualProblem = actualContest.Problems[i];
                Problem expectedProblem = expectedProblems[i];
                Assert.Equal(expectedProblem.Name, actualProblem.Name);
                Assert.Equal(expectedProblem.Id, actualProblem.Id);
                Assert.Equal(expectedProblem.IsExtraTask, actualProblem.IsExtraTask);
            }
            Assert.Equal(2, actualContest.ContestResults.Count);
            ContestResultViewModel naskoResults = actualContest.ContestResults[0];
            Assert.Equal(50, naskoResults.PointsByProblem[1]);
            Assert.Equal(100, naskoResults.PointsByProblem[3]);
            Assert.Equal(150, naskoResults.Total);
            Assert.Equal(11, naskoResults.Student.ClassNumber);
            Assert.Equal("A", naskoResults.Student.ClassType);
            Assert.Equal(2, naskoResults.Student.NumberInCalss);
            Assert.Equal("Atanas Vasilev", naskoResults.Student.FullName);
            ContestResultViewModel martoResults = actualContest.ContestResults[1];
            Assert.Equal(60, martoResults.PointsByProblem[1]);
            Assert.Equal(100, martoResults.PointsByProblem[2]);
            Assert.Equal(70, martoResults.PointsByProblem[3]);
            Assert.Equal(230, martoResults.Total);
            Assert.Equal(12, martoResults.Student.ClassNumber);
            Assert.Equal("B", martoResults.Student.ClassType);
            Assert.Equal(17, martoResults.Student.NumberInCalss);
            Assert.Equal("Marto Martinov", martoResults.Student.FullName);
        }

        [Fact]
        public async Task GetContestReults_WithInvalidContestId_ShouldThrowEntityNotFoundException()
        {
            List<Contest> testData = GetContestReultsTestData();
            ContestService service = await CreateContestService(testData);

            Assert.Throws<EntityNotFoundException>(() => service.GetContestReults(564, 12));
        }

        [Theory]
        [InlineData(10)]
        [InlineData(60)]
        [InlineData(65)]
        public void GetNumberOfPages_WithDifferentData_ShouldWorkCorrect(int userContestsCount)
        {
            Contest contest = GenerateContestForGetContestResultsPagesCount(userContestsCount);
            var testData = new List<Contest> { GenerateContestForGetContestResultsPagesCount(userContestsCount) };
            ContestService service = CreateContestServiceWithMockedRepository(testData.AsQueryable());

            int expectedPages = service.GetContestResultsPagesCount(contest.Id);

            Assert.Equal(paginationService.CalculatePagesCount(userContestsCount, ContestService.ResultsPerPage), expectedPages);
        }

        [Fact]
        public async Task GetLessonId_WithInvalidId_ShouldThrowEntityNotFoundException()
        {
            ContestService contestService = await CreateContestService(GetTestData());

            await Assert.ThrowsAsync<EntityNotFoundException>(() => contestService.GetLessonId(999));
        }

        [Fact]
        public async Task GetLessonId_WithValidId_ShouldReturnCottectData()
        {
            var testData = new List<Contest> { new Contest { Id = 1, LessonId = 10 } };
            ContestService contestService = await CreateContestService(testData);

            int expectedLessonId = await contestService.GetLessonId(1);

            Assert.Equal(10, expectedLessonId);
        }

        [Theory]
        [InlineData(115)]
        [InlineData(null)]
        public async Task GetContestSubmissions_WithMockedDependencies_ShouldWorkCorrect(int? problemId)
        {
            //Arrange
            int lessonId = 10;
            int baseProblemId = problemId ?? 200;
            int contestId = 1;
            string baseUrl = "baseUrl/test?contestId=" + contestId;
            string problemName = "Mocking";
            string userId = "nasko_user";
            int page = 3;
            int totalSubmissions = 20;
            int pagesCount = 7;
            var submissionResults = new List<SubmissionResult>();
            for (int i = 0; i < 3; i++)
            {
                submissionResults.Add(new SubmissionResult { ActualPoints = i + 50, Id = i });
            }

            await context.Contests.AddAsync(new Contest { Id = contestId, LessonId = lessonId });
            await context.SaveChangesAsync();
            var repository = new EfDeletableEntityRepository<Contest>(context);

            var lessonServiceMock = new Mock<ILessonService>();
            lessonServiceMock.Setup(x => x.GetFirstProblemId(lessonId)).Returns(baseProblemId);

            var problemServiceMock = new Mock<IProblemService>();
            problemServiceMock.Setup(x => x.GetProblemName(baseProblemId)).Returns(problemName);

            var submissionServiceMock = new Mock<ISubmissionService>();
            submissionServiceMock.Setup(x =>
                x.GetUserSubmissionsByProblemIdAndContestId(contestId, baseProblemId, userId, page, It.IsAny<int>())).Returns(submissionResults);
            submissionServiceMock.Setup(x => 
                x.GetSubmissionsCountByProblemIdAndContestId(baseProblemId, contestId, userId)).Returns(totalSubmissions);

            var paginationServiceMock = new Mock<IPaginationService>();
            paginationServiceMock.Setup(x => 
                x.CalculatePagesCount(totalSubmissions, It.IsAny<int>())).Returns(pagesCount);
            var service = new ContestService(repository, null, lessonServiceMock.Object, problemServiceMock.Object, 
                submissionServiceMock.Object, paginationServiceMock.Object);

            //Act
            ContestSubmissionsViewModel actualModel = await service.GetContestSubmissions(contestId, userId, problemId, page, baseUrl);
            string expecedUrlPlaceholder = baseUrl + $"{GlobalConstants.QueryStringDelimiter}{GlobalConstants.ProblemIdKey}=" + "{0}";
            string expecedPaginationUrl = baseUrl + $"{GlobalConstants.QueryStringDelimiter}{GlobalConstants.ProblemIdKey}={baseProblemId}{GlobalConstants.QueryStringDelimiter}{GlobalConstants.PageKey}=" + "{0}";

            //Assert
            Assert.Equal(problemName, actualModel.ProblemName);
            Assert.Equal(lessonId, actualModel.LessonId);
            Assert.Equal(submissionResults.Count, actualModel.Submissions.Count());
            Assert.Equal(expecedUrlPlaceholder, actualModel.UrlPlaceholder);
            Assert.Equal(expecedPaginationUrl, actualModel.PaginationData.Url);
            Assert.Equal(page, actualModel.PaginationData.CurrentPage);
            Assert.Equal(pagesCount, actualModel.PaginationData.NumberOfPages);
        }

        private async Task<ContestService> CreateContestService(List<Contest> testData, IRepository<UserContest> userContestRepository)
        {
            await context.Contests.AddRangeAsync(testData);
            await context.SaveChangesAsync();
            IDeletableEntityRepository<Contest> repository = new EfDeletableEntityRepository<Contest>(context);
            var service = new ContestService(repository, userContestRepository, null, null, null, paginationService);
            return service;
        }

        private async Task<ContestService> CreateContestService(List<Contest> testData) => 
            await CreateContestService(testData, null);

        private ContestService CreateContestServiceWithMockedRepository(IQueryable<Contest> testData, IRepository<UserContest> userContestRepository)
        {
            var reposotiryMock = new Mock<IDeletableEntityRepository<Contest>>();
            reposotiryMock.Setup(x => x.All()).Returns(testData);
            return new ContestService(reposotiryMock.Object, userContestRepository, null, null, null, paginationService);
        }

        private ContestService CreateContestServiceWithMockedRepository(IQueryable<Contest> testData) => 
            CreateContestServiceWithMockedRepository(testData, null);

        private List<UserContest> GetUserContestsTestData()
        {
            var contests = new List<UserContest>
            {
                new UserContest { ContestId = 1, UserId = "user_id_1" },
                new UserContest { ContestId = 2, UserId = "user_id_20" },
            };
            return contests;
        }

        private List<Contest> GetContestsTestData()
        {
            var contests = new List<Contest>
            {
                new Contest{Id = 1, Name = "contest1", EndTime = new DateTime(2019, 12, 20), StartTime = new DateTime(2019, 07, 05)},
                new Contest{Id = 2, Name = "contest2", EndTime = new DateTime(2019, 05, 20), StartTime = new DateTime(2019, 04, 05)},
            };
            return contests;
        }

        private List<Contest> GetTestData()
        {
            var lesson = new Lesson { Id = 10, Practice = new Practice() };
            return new List<Contest>()
            {
                new Contest { Id = 1,  Name = "compete1", EndTime = DateTime.Now.AddDays(1), StartTime = DateTime.Now.AddDays(-1), Lesson = lesson },
                new Contest { Id = 2,  Name = "compete2", EndTime = DateTime.Now.AddDays(-4), StartTime = DateTime.Now.AddDays(-10), Lesson = lesson },
                new Contest { Id = 3,  Name = "compete3", EndTime = DateTime.Now.AddMinutes(10), StartTime = DateTime.Now.AddHours(-1), Lesson = lesson },
                new Contest { Id = 4,  Name = "compete4", EndTime = DateTime.Now.AddDays(10), StartTime = DateTime.Now.AddHours(1), Lesson = lesson },
                new Contest { Id = 5,  Name = "compete5", EndTime = DateTime.Now.AddDays(-2), StartTime = DateTime.Now.AddHours(-5), Lesson = lesson },
            };
        }

        private IEnumerable<Contest> Generate50ContestsWithStartDate()
        {
            for (int i = 1; i <= 50; i++)
            {
                yield return new Contest { Id = 50 - i, StartTime = DateTime.UtcNow.AddDays(i) };
            }
        }

        private List<Contest> GetContestReultsTestData()
        {
            const int contestId = 11;
            var lesson = new Lesson { Id = 10 };
            var problems = new List<Problem>
            {
                new Problem { Id = 1, Name = "problem1", LessonId = lesson.Id, IsExtraTask = false },
                new Problem { Id = 2, Name = "problem2", LessonId = lesson.Id, IsExtraTask = false },
                new Problem { Id = 3, Name = "problem3", LessonId = lesson.Id, IsExtraTask = true },
            };
            lesson.Problems = problems;

            var naskoStudent = new Student
            {
                Id = "nasko-student",
                SchoolClass = new SchoolClass { ClassNumber = 11, ClassType = SchoolClassType.A },
                NumberInCalss = 2,
                FullName = "Atanas Vasilev",
            };
            var martoStudent = new Student
            {
                Id = "marto-student",
                SchoolClass = new SchoolClass { ClassNumber = 12, ClassType = SchoolClassType.B },
                NumberInCalss = 17,
                FullName = "Marto Martinov",
            };

            return new List<Contest>()
            {
                new Contest
                {
                    Id = contestId,
                    Name = "contestResults",
                    Lesson = lesson,
                    UserContests = new List<UserContest>
                    {
                        new UserContest
                        {
                            User = new ApplicationUser
                            {
                                StudentId = naskoStudent.Id,
                                UserName = "nasko",
                                Student = naskoStudent,
                                Submissions = new List<Submission>
                                {
                                    new Submission { ContestId = contestId, ProblemId =  problems[0].Id, ActualPoints = 40},
                                    new Submission { ContestId = contestId, ProblemId =  problems[0].Id, ActualPoints = 50},
                                    new Submission { ContestId = 12, ProblemId =  15, ActualPoints = 100},
                                    new Submission { ContestId = contestId, ProblemId =  problems[2].Id, ActualPoints = 100},
                                    new Submission { ContestId = contestId, ProblemId =  problems[2].Id, ActualPoints = 100},
                                    new Submission { ContestId = contestId, ProblemId =  problems[2].Id, ActualPoints = 20},
                                }
                            },
                            ContestId = contestId
                        },
                        new UserContest
                        {
                            User = new ApplicationUser
                            {
                                StudentId = martoStudent.Id,
                                UserName = "marto",
                                Student = martoStudent,
                                Submissions = new List<Submission>
                                {
                                    new Submission { ContestId = contestId, ProblemId =  problems[0].Id, ActualPoints = 40},
                                    new Submission { ContestId = contestId, ProblemId =  problems[0].Id, ActualPoints = 60},
                                    new Submission { ContestId = 12, ProblemId =  15, ActualPoints = 100},
                                    new Submission { ContestId = 123, ProblemId =  155, ActualPoints = 50},
                                    new Submission { ContestId = contestId, ProblemId =  problems[1].Id, ActualPoints = 100},
                                    new Submission { ContestId = contestId, ProblemId =  problems[1].Id, ActualPoints = 50},
                                    new Submission { ContestId = contestId, ProblemId =  problems[2].Id, ActualPoints = 70},
                                }
                            },
                            ContestId = contestId
                        },
                        new UserContest
                        {
                            User = new ApplicationUser
                            {
                                UserName = "not-students",
                            },
                            ContestId = contestId
                        }
                    }
                }
            };
        }

        private Contest GenerateContestForGetContestResultsPagesCount(int userContestsCount)
        {
            var userContests = new List<UserContest>();
            for (int i = 0; i < userContestsCount; i++)
            {
                userContests.Add(new UserContest
                {
                    User = new ApplicationUser
                    {
                        StudentId = "test" + i,
                    }
                });
            }

            for (int i = 0; i < 10; i++)
            {
                userContests.Add(new UserContest
                {
                    User = new ApplicationUser
                    {
                        StudentId = null
                    }
                });
            }

            return new Contest { Id = 1, UserContests = userContests };
        }
    }
}
