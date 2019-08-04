using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Data.Repositories;
using JudgeSystem.Web.ViewModels.User;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace JudgeSystem.Services.Data.Tests
{
    public class UserServiceTests : TransientDbContextProvider
    {
        [Fact]
        public void GetContestResults_WithValidData_ShouldWorkCorerct()
        {
            var testData = GetContestResultsTestData();
            var service = CreateUserServiceWithMockedRepository(testData.AsQueryable());

            var actualResults = service.GetContestResults("test_user");
            var expectedResult = testData.First().UserContests.Select(s => s.Contest).ToList();

            Assert.Equal(2, actualResults.Count);
            var firstResult = actualResults[0];
            var secondReslut = actualResults[1];
            Assert.Equal(200, firstResult.MaxPoints);
            Assert.Equal(100, firstResult.ActualPoints);
            Assert.Equal("OOP Basics", firstResult.ContestName);
            Assert.Equal(1, firstResult.ContestId);
            Assert.Equal(2, firstResult.LessonId);
            Assert.Equal(200, secondReslut.MaxPoints);
            Assert.Equal(130, secondReslut.ActualPoints);
            Assert.Equal("Programming Basics", secondReslut.ContestName);
            Assert.Equal(2, secondReslut.ContestId);
            Assert.Equal(1, secondReslut.LessonId);
        }

        [Fact]
        public void GetPracticeResults_WithValidData_ShouldWorkCorerct()
        {
            var testData = GetPracticeResultsTestData();
            var service = CreateUserServiceWithMockedRepository(testData.AsQueryable());

            var actualResults = service.GetPracticeResults("test_user");
            var expectedResult = testData.First().UserPractices.Select(s => s.Practice).ToList();

            Assert.Equal(2, actualResults.Count);
            var firstResult = actualResults[0];
            var secondReslut = actualResults[1];
            Assert.Equal(200, firstResult.MaxPoints);
            Assert.Equal(200, firstResult.ActualPoints);
            Assert.Equal(2, firstResult.LessonId);
            Assert.Equal(1, firstResult.PracticeId);
            Assert.Equal("OOP", firstResult.LessonName);
            Assert.Equal(200, secondReslut.MaxPoints);
            Assert.Equal(170, secondReslut.ActualPoints);
            Assert.Equal(1, secondReslut.LessonId);
            Assert.Equal(2, secondReslut.PracticeId);
            Assert.Equal("C#", secondReslut.LessonName);

        }

        [Fact]
        public void GetExamResluts_WithValidData_ShouldWorkCorrect()
        {
            var testData = GetUserExamResultsTestData();
            var service = CreateUserServiceWithMockedRepository(testData.AsQueryable());

            var actualResult = service.GetUserExamResults("test_user").ToList();

            Assert.Equal(2, actualResult.Count);
            var firstExamResult = actualResult[0];
            var secondExamResult = actualResult[1];
            Assert.Equal(180, firstExamResult.ActualPoints);
            Assert.Equal(200, firstExamResult.MaxPoints);
            Assert.Equal(1, firstExamResult.LessonId);
            Assert.Equal("Programming Basics", firstExamResult.ContestName);
            Assert.Equal(250, secondExamResult.ActualPoints);
            Assert.Equal(300, secondExamResult.MaxPoints);
            Assert.Equal(3, secondExamResult.LessonId);
            Assert.Equal("Exam contest", secondExamResult.ContestName);
        }

        private UserService CreateUserServiceWithMockedRepository(IQueryable<ApplicationUser> testData)
        {
            var reposotiryMock = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            reposotiryMock.Setup(x => x.All()).Returns(testData);
            return new UserService(reposotiryMock.Object);
        }

        private List<ApplicationUser> GetContestResultsTestData()
        {
            Problem sumTwoNumbers = new Problem { Id = 1, Name = "Sum two numbers", IsExtraTask = true, MaxPoints = 100 };
            Problem helloWorld = new Problem { Id = 2, Name = "Hello world",  IsExtraTask = false, MaxPoints = 100 };
            Problem multiply = new Problem { Id = 3, Name = "Multiply", IsExtraTask = false, MaxPoints = 100 };
            Problem interfaces = new Problem { Id = 4, Name = "Interfaces", IsExtraTask = false, MaxPoints = 100 };
            Problem inheritance = new Problem { Id = 5, Name = "Inheritance", IsExtraTask = false, MaxPoints = 100 };

            Lesson oopLesson = new Lesson
            {
                Id = 2,
                Name = "OOP",
                Problems = new List<Problem> { interfaces, inheritance }
            };
            Lesson cSharpLesson = new Lesson
            {
                Id = 1,
                Name = "C#",
                Problems = new List<Problem> { sumTwoNumbers, helloWorld, multiply }
            };

            return new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "test_user",
                    UserName = "atanas",
                    UserContests = new List<UserContest>()
                    {
                        new UserContest
                        {
                            Contest = new Contest
                            {
                                Id = 1,
                                Name = "OOP Basics",
                                Submissions = new List<Submission>()
                                {
                                    new Submission { ProblemId = interfaces.Id, Problem = interfaces, ActualPoints = 80, UserId = "test_user" },
                                    new Submission { ProblemId = inheritance.Id, Problem = inheritance, ActualPoints = 50, UserId = "test_user2" },
                                    new Submission { ProblemId = interfaces.Id, Problem = interfaces, ActualPoints = 70, UserId = "test_user" },
                                    new Submission { ProblemId = interfaces.Id, Problem = interfaces, ActualPoints = 100, UserId = "test_user" },
                                },
                                Lesson = oopLesson,
                                LessonId = oopLesson.Id
                            }
                        },
                        new UserContest
                        {
                            Contest = new Contest
                            {
                                Id = 2,
                                Name = "Programming Basics",
                                Submissions = new List<Submission>()
                                {
                                    new Submission { ProblemId = sumTwoNumbers.Id, Problem = sumTwoNumbers, ActualPoints = 100, UserId = "test_user" },
                                    new Submission { ProblemId = sumTwoNumbers.Id, Problem = sumTwoNumbers, ActualPoints = 70, UserId = "test_user" },
                                    new Submission { ProblemId = helloWorld.Id, Problem = helloWorld, ActualPoints = 70, UserId = "test_user" },
                                    new Submission { ProblemId = helloWorld.Id, Problem = helloWorld, ActualPoints = 80, UserId = "test_user" },
                                    new Submission { ProblemId = multiply.Id, Problem = multiply, ActualPoints = 50, UserId = "test_user" },
                                    new Submission { ProblemId = multiply.Id, Problem = multiply, ActualPoints = 100, UserId = "test_user2" },
                                },
                                Lesson = cSharpLesson,
                                LessonId = cSharpLesson.Id
                            }
                        }
                    }
                },
                new ApplicationUser
                {
                    Id = "test_userr"
                }
            };
        }

        private List<ApplicationUser> GetPracticeResultsTestData()
        {
            Problem sumTwoNumbers = new Problem { Id = 1, Name = "Sum two numbers", IsExtraTask = true, MaxPoints = 100 };
            Problem helloWorld = new Problem { Id = 2, Name = "Hello world", IsExtraTask = false, MaxPoints = 100 };
            Problem multiply = new Problem { Id = 3, Name = "Multiply", IsExtraTask = false, MaxPoints = 100 };
            Problem interfaces = new Problem { Id = 4, Name = "Interfaces", IsExtraTask = false, MaxPoints = 100 };
            Problem inheritance = new Problem { Id = 5, Name = "Inheritance", IsExtraTask = false, MaxPoints = 100 };

            Lesson oopLesson = new Lesson
            {
                Id = 2,
                Name = "OOP",
                Problems = new List<Problem> { interfaces, inheritance }
            };
            Lesson cSharpLesson = new Lesson
            {
                Id = 1,
                Name = "C#",
                Problems = new List<Problem> { sumTwoNumbers, helloWorld, multiply }
            };

            return new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "test_user",
                    UserName = "atanas",
                    UserPractices = new List<UserPractice>()
                    {
                        new UserPractice
                        {
                            Practice = new Practice
                            {
                                Id = 1,
                                Submissions = new List<Submission>()
                                {
                                    new Submission { ProblemId = interfaces.Id, Problem = interfaces, ActualPoints = 80, UserId = "test_user" },
                                    new Submission { ProblemId = inheritance.Id, Problem = inheritance, ActualPoints = 50, UserId = "test_user2" },
                                    new Submission { ProblemId = inheritance.Id, Problem = inheritance, ActualPoints = 50, UserId = "test_user" },
                                    new Submission { ProblemId = inheritance.Id, Problem = inheritance, ActualPoints = 100, UserId = "test_user" },
                                    new Submission { ProblemId = interfaces.Id, Problem = interfaces, ActualPoints = 70, UserId = "test_user" },
                                    new Submission { ProblemId = interfaces.Id, Problem = interfaces, ActualPoints = 100, UserId = "test_user" },
                                },
                                Lesson = oopLesson,
                                LessonId = oopLesson.Id
                            }
                        },
                        new UserPractice
                        {
                            Practice = new Practice
                            {
                                Id = 2,
                                Submissions = new List<Submission>()
                                {
                                    new Submission { ProblemId = sumTwoNumbers.Id, Problem = sumTwoNumbers, ActualPoints = 100, UserId = "test_user" },
                                    new Submission { ProblemId = sumTwoNumbers.Id, Problem = sumTwoNumbers, ActualPoints = 70, UserId = "test_user" },
                                    new Submission { ProblemId = sumTwoNumbers.Id, Problem = sumTwoNumbers, ActualPoints = 100, UserId = "test_user2" },
                                    new Submission { ProblemId = helloWorld.Id, Problem = helloWorld, ActualPoints = 70, UserId = "test_user" },
                                    new Submission { ProblemId = helloWorld.Id, Problem = helloWorld, ActualPoints = 80, UserId = "test_user" },
                                    new Submission { ProblemId = helloWorld.Id, Problem = helloWorld, ActualPoints = 80, UserId = "test_user2" },
                                    new Submission { ProblemId = helloWorld.Id, Problem = helloWorld, ActualPoints = 100, UserId = "test_user2" },
                                    new Submission { ProblemId = multiply.Id, Problem = multiply, ActualPoints = 50, UserId = "test_user" },
                                    new Submission { ProblemId = multiply.Id, Problem = multiply, ActualPoints = 90, UserId = "test_user" },
                                    new Submission { ProblemId = multiply.Id, Problem = multiply, ActualPoints = 90, UserId = "test_user" },
                                    new Submission { ProblemId = multiply.Id, Problem = multiply, ActualPoints = 100, UserId = "test_user2" },
                                },
                                Lesson = cSharpLesson,
                                LessonId = cSharpLesson.Id
                            }
                        }
                    }
                },
                new ApplicationUser
                {
                    Id = "test_userr"
                }
            };
        }

        private List<ApplicationUser> GetUserExamResultsTestData()
        {
            Problem sumTwoNumbers = new Problem { Id = 1, Name = "Sum two numbers", IsExtraTask = true, MaxPoints = 100 };
            Problem helloWorld = new Problem { Id = 2, Name = "Hello world", IsExtraTask = false, MaxPoints = 100 };
            Problem multiply = new Problem { Id = 3, Name = "Multiply", IsExtraTask = false, MaxPoints = 100 };
            Problem interfaces = new Problem { Id = 4, Name = "Interfaces", IsExtraTask = false, MaxPoints = 100 };
            Problem inheritance = new Problem { Id = 5, Name = "Inheritance", IsExtraTask = false, MaxPoints = 100 };
            Problem examProblem = new Problem { Id = 6, Name = "Inheritance", IsExtraTask = false, MaxPoints = 300 };

            Lesson oopLesson = new Lesson
            {
                Id = 2,
                Name = "OOP",
                Problems = new List<Problem> { interfaces, inheritance },
                Type = LessonType.Exercise
            };
            Lesson cSharpLesson = new Lesson
            {
                Id = 1,
                Name = "C#",
                Problems = new List<Problem> { sumTwoNumbers, helloWorld, multiply },
                Type = LessonType.Exam
            };
            Lesson examLesson = new Lesson
            {
                Id = 3,
                Name = "C#",
                Problems = new List<Problem> { examProblem },
                Type = LessonType.Exam
            };

            return new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = "test_user",
                    UserName = "atanas",
                    UserContests = new List<UserContest>()
                    {
                        new UserContest
                        {
                            Contest = new Contest
                            {
                                Name = "OOP Basics",
                                Submissions = new List<Submission>()
                                {
                                    new Submission { ProblemId = interfaces.Id, Problem = interfaces, ActualPoints = 80, UserId = "test_user" },
                                    new Submission { ProblemId = interfaces.Id, Problem = interfaces, ActualPoints = 100, UserId = "test_usdsser" },
                                },
                                Lesson = oopLesson,
                                LessonId = oopLesson.Id
                            }
                        },
                        new UserContest
                        {
                            Contest = new Contest
                            {
                                Name = "Programming Basics",
                                Submissions = new List<Submission>()
                                {
                                    new Submission { ProblemId = sumTwoNumbers.Id, Problem = sumTwoNumbers, ActualPoints = 70, UserId = "test_user" },
                                    new Submission { ProblemId = sumTwoNumbers.Id, Problem = sumTwoNumbers, ActualPoints = 100, UserId = "test_user" },
                                    new Submission { ProblemId = sumTwoNumbers.Id, Problem = sumTwoNumbers, ActualPoints = 70, UserId = "test_use2r" },
                                    new Submission { ProblemId = multiply.Id, Problem = multiply, ActualPoints = 100, UserId = "test_user2" },
                                    new Submission { ProblemId = multiply.Id, Problem = multiply, ActualPoints = 80, UserId = "test_user" },
                                    new Submission { ProblemId = helloWorld.Id, Problem = helloWorld, ActualPoints = 0, UserId = "test_user" },
                                    new Submission { ProblemId = helloWorld.Id, Problem = helloWorld, ActualPoints = 100, UserId = "test_user" },
                                    new Submission { ProblemId = helloWorld.Id, Problem = helloWorld, ActualPoints = 80, UserId = "test_user" },
                                    new Submission { ProblemId = helloWorld.Id, Problem = helloWorld, ActualPoints = 100, UserId = "test_user2" },
                                },
                                Lesson = cSharpLesson,
                                LessonId = cSharpLesson.Id
                            }
                        },
                        new UserContest
                        {
                            Contest = new Contest
                            {
                                Name = "Exam contest",
                                Submissions = new List<Submission>()
                                {
                                    new Submission { ProblemId = examProblem.Id, Problem = examProblem, ActualPoints = 70, UserId = "test_user" },
                                    new Submission { ProblemId = examProblem.Id, Problem = examProblem, ActualPoints = 250, UserId = "test_user" },
                                    new Submission { ProblemId = examProblem.Id, Problem = examProblem, ActualPoints = 200, UserId = "test_user" },
                                    new Submission { ProblemId = examProblem.Id, Problem = examProblem, ActualPoints = 100, UserId = "test_user2" },
                                    new Submission { ProblemId = examProblem.Id, Problem = examProblem, ActualPoints = 300, UserId = "test_user2" },
                                },
                                Lesson = examLesson,
                                LessonId = examLesson.Id
                            }
                        }
                    }
                },
                new ApplicationUser
                {
                    Id = "test_userr"
                }
            };
        }
    }
}
