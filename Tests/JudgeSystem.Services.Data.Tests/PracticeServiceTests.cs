using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Common.Exceptions;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Repositories;
using JudgeSystem.Web.Dtos.Lesson;
using JudgeSystem.Web.ViewModels.Practice;
using JudgeSystem.Web.ViewModels.Problem;

using Xunit;

namespace JudgeSystem.Services.Data.Tests
{
    public class PracticeServiceTests : TransientDbContextProvider
    {
        [Fact]
        public async Task AddUserToPracticeIfNotAdded_WithNonExistingUser_ShouldWorkCorrect()
        {
            IRepository<UserPractice> userPracticeRepository = new EfRepository<UserPractice>(context);
            PracticeService service = await CreatePracticeService(GetTestData(), userPracticeRepository);

            await service.AddUserToPracticeIfNotAdded("test123", 2);

            Assert.Contains(context.UserPractices, x => x.PracticeId == 2 && x.UserId == "test123");
        }

        [Fact]
        public async Task AddUserToPracticeIfNotAdded_WithExistingUserPractice_ShouldDoNothing()
        {
            await context.UserPractices.AddAsync(new UserPractice { UserId = "test", PracticeId = 2 });
            IRepository<UserPractice> userPracticeRepository = new EfRepository<UserPractice>(context);
            PracticeService service = await CreatePracticeService(GetTestData(), userPracticeRepository);

            await service.AddUserToPracticeIfNotAdded("test", 2);

            Assert.True(context.UserPractices.Count() == 1);
        }

        [Fact]
        public async Task Create_WithValidData_ShouldWorkCorrect()
        {
            PracticeService service = await CreatePracticeService(new List<Practice>(), null);

            await service.Create(5);

            Assert.Contains(context.Practices, x => x.LessonId == 5);
        }

        [Fact]
        public async Task GetLesson_WithValidData_ShouldWorkCorrect()
        {
            PracticeService service = await CreatePracticeService(GetTestData(), null);

            LessonDto lesson = await service.GetLesson(2);

            Assert.Equal(45, lesson.Id);
            Assert.Equal("Test", lesson.Name);
        }

        [Fact]
        public async Task GetLesson_WithInValidData_ShouldThrowEntityNotFoundException()
        {
            PracticeService service = await CreatePracticeService(GetTestData(), null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetLesson(22));
        }

        [Fact]
        public async Task GetPracticeReults_WithValidPracticeId_ShouldReturnCorrectData()
        {
            //Arrange
            List<Practice> testData = GetPracticeReultsTestData();
            PracticeService service = await CreatePracticeService(testData, null);
            Practice expectedPractice = testData.First(); 
            var expectedProblems = expectedPractice.Lesson.Problems.OrderBy(x => x.CreatedOn).ToList();
            ApplicationUser firstUser = expectedPractice.UserPractices.First().User;
            ApplicationUser otherUser = expectedPractice.UserPractices.Skip(1).First().User;

            //Act
            PracticeAllResultsViewModel actualPracticeResult = service.GetPracticeResults(expectedPractice.Id, 1, 10);

            //Assert
            Assert.Equal(expectedPractice.Lesson.Name, actualPracticeResult.LessonName);
            Assert.Equal(expectedProblems.Count, actualPracticeResult.Problems.Count);
            
            for (int i = 0; i < actualPracticeResult.Problems.Count; i++)
            {
                PracticeProblemViewModel actualProblem = actualPracticeResult.Problems[i];
                Problem expectedProblem = expectedProblems[i];

                Assert.Equal(actualPracticeResult.ProblemsIds[i], actualProblem.Id);
                Assert.Equal(expectedProblem.Name, actualProblem.Name);
                Assert.Equal(expectedProblem.Id, actualProblem.Id);
                Assert.Equal(expectedProblem.MaxPoints, actualProblem.MaxPoints);
                Assert.Equal(expectedProblem.IsExtraTask, actualProblem.IsExtraTask);
            }

            Assert.Equal(2, actualPracticeResult.PracticeResults.Count);

            PracticeResultViewModel firstUserResults = actualPracticeResult.PracticeResults[1];
            Assert.Equal(50, firstUserResults.PointsByProblem[1]);
            Assert.Equal(100, firstUserResults.PointsByProblem[3]);
            Assert.Equal(150, firstUserResults.Total);
            Assert.Equal($"{firstUser.Name} {firstUser.Surname}", firstUserResults.FullName);
            Assert.Equal(firstUser.UserName, firstUserResults.Username);
            Assert.Equal(firstUser.Id, firstUserResults.UserId);

            PracticeResultViewModel otherUserResults = actualPracticeResult.PracticeResults[0];
            Assert.Equal(60, otherUserResults.PointsByProblem[1]);
            Assert.Equal(100, otherUserResults.PointsByProblem[2]);
            Assert.Equal(70, otherUserResults.PointsByProblem[3]);
            Assert.Equal(230, otherUserResults.Total);
            Assert.Equal($"{otherUser.Name} {otherUser.Surname}", otherUserResults.FullName);
            Assert.Equal(otherUser.UserName, otherUserResults.Username);
            Assert.Equal(otherUser.Id, otherUserResults.UserId);
        }

        [Theory]
        [InlineData(1, 50)]
        [InlineData(1, 20)]
        [InlineData(3, 20)]
        [InlineData(1, 1)]
        [InlineData(3, 10)]
        public async Task GetPracticeReults_WithDifferentArguments_ShouldReturnOnlyResultsForSelectedPage(int page, int entitesPerPage)
        {
            //Arrange
            Practice practice = GeneratePracticeResults();
            PracticeService service = await CreatePracticeService(new List<Practice> { practice }, null);
            var userPractices = practice.UserPractices.OrderBy(x => x.User.UserName).Skip((page - 1) * entitesPerPage).Take(entitesPerPage).ToList();

            //Act
            List<PracticeResultViewModel> practiceResults = service.GetPracticeResults(practice.Id, page, entitesPerPage).PracticeResults;

            //Assert
            for (int i = 0; i < practiceResults.Count; i++)
            {
                Assert.Equal(userPractices[i].User.Id, practiceResults[i].UserId);
            }
            
        }

        [Theory]
        [InlineData(40, 40, 1)]
        [InlineData(60, 40, 1)]
        [InlineData(60, 120, 2)]
        [InlineData(60, 150, 3)]
        [InlineData(1, 150, 150)]
        public async Task GetPracticeResultsPagesCount_WithDifferentArguments_ShouldReturnPagesCount(int entitesPerPage, int entitiesCount, int expectedPagesCount)
        {
            IEnumerable<UserPractice> userPractices = GenerateUserPractices(entitiesCount);
            await context.UserPractices.AddRangeAsync(userPractices);
            await context.SaveChangesAsync();
            IRepository<UserPractice> userPracticeRepository = new EfRepository<UserPractice>(context);
            var service = new PracticeService(null, userPracticeRepository, new PaginationService());

            int actualPagesCount = service.GetPracticeResultsPagesCount(userPractices.First().PracticeId, entitesPerPage);

            Assert.Equal(expectedPagesCount, actualPagesCount);
        }

        private async Task<PracticeService> CreatePracticeService(List<Practice> testData, IRepository<UserPractice> userPracticeRepository)
        {
            await context.Practices.AddRangeAsync(testData);
            await context.SaveChangesAsync();
            IDeletableEntityRepository<Practice> repository = new EfDeletableEntityRepository<Practice>(context);
            var service = new PracticeService(repository, userPracticeRepository, new PaginationService());
            return service;
        }

        private List<Practice> GetTestData()
        {
            var practices = new List<Practice>
            {
                new Practice{ Id = 1, Lesson = new Lesson{ Name = "Test123", Id = 1 } },
                new Practice{ Id = 2, Lesson = new Lesson{ Name = "Test", Id = 45 } },
                new Practice{ Id = 3, Lesson = new Lesson{ Name = "Test321", Id = 3 } },
            };
            return practices;
        }

        private IEnumerable<UserPractice> GenerateUserPractices(int entitesCount)
        {
            for (int i = 0; i < entitesCount * 2; i++)
            {
                int practiceId = (i % 2) + 1;

                yield return new UserPractice
                {
                    PracticeId = practiceId,
                    UserId = $"id_{i + 1}"
                };
            }
        }

        private List<Practice> GetPracticeReultsTestData()
        {
            const int practiceId = 11;
            var lesson = new Lesson { Id = 10 };
            var problems = new List<Problem>
            {
                new Problem { Id = 1, Name = "problem1", LessonId = lesson.Id, IsExtraTask = false },
                new Problem { Id = 2, Name = "problem2", LessonId = lesson.Id, IsExtraTask = false },
                new Problem { Id = 3, Name = "problem3", LessonId = lesson.Id, IsExtraTask = true },
            };

            lesson.Problems = problems;

            return new List<Practice>()
            {
                new Practice
                {
                    Id = practiceId,
                    Lesson = lesson,
                    UserPractices = new List<UserPractice>
                    {
                        new UserPractice
                        {
                            User = new ApplicationUser
                            {
                                Id = "nasko_id",
                                Name = "Atanas",
                                Surname = "Vasilev",
                                UserName = "nasko",
                                Submissions = new List<Submission>
                                {
                                    new Submission { PracticeId = practiceId, ProblemId =  problems[0].Id, ActualPoints = 40},
                                    new Submission { PracticeId = practiceId, ProblemId =  problems[0].Id, ActualPoints = 50},
                                    new Submission { PracticeId = 12, ProblemId =  15, ActualPoints = 100},
                                    new Submission { PracticeId = practiceId, ProblemId =  problems[2].Id, ActualPoints = 100},
                                    new Submission { PracticeId = practiceId, ProblemId =  problems[2].Id, ActualPoints = 100},
                                    new Submission { PracticeId = practiceId, ProblemId =  problems[2].Id, ActualPoints = 20},
                                }
                            },
                            PracticeId = practiceId
                        },
                        new UserPractice
                        {
                            User = new ApplicationUser
                            {
                                Id = "marto_id",
                                Name = "Marto",
                                Surname = "Martinov",
                                UserName = "marto",
                                Submissions = new List<Submission>
                                {
                                    new Submission { PracticeId = practiceId, ProblemId =  problems[0].Id, ActualPoints = 40},
                                    new Submission { PracticeId = practiceId, ProblemId =  problems[0].Id, ActualPoints = 60},
                                    new Submission { PracticeId = 12, ProblemId =  15, ActualPoints = 100},
                                    new Submission { PracticeId = 123, ProblemId =  155, ActualPoints = 50},
                                    new Submission { PracticeId = practiceId, ProblemId =  problems[1].Id, ActualPoints = 100},
                                    new Submission { PracticeId = practiceId, ProblemId =  problems[1].Id, ActualPoints = 50},
                                    new Submission { PracticeId = practiceId, ProblemId =  problems[2].Id, ActualPoints = 70},
                                }
                            },
                            PracticeId = practiceId
                        }
                    }
                },
                new Practice
                {
                    Id = 99
                }
            };
        }

        private static Practice GeneratePracticeResults()
        {
            var practice = new Practice
            {
                Lesson = new Lesson
                {
                    Id = 1,
                    Name = "Test"
                },
                Id = 1
            };

            for (int i = 0; i < 50; i++)
            {
                var userPractice = new UserPractice
                {
                    User = new ApplicationUser
                    {
                        Id = $"id_{i}",
                        UserName = $"user_{i}",
                        Name = $"name_{i}",
                        Surname = $"surname_{i}"
                    }
                };

                practice.UserPractices.Add(userPractice);
            };

            return practice;
        }
    }
}
