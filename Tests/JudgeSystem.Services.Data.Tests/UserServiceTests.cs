using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Data.Repositories;
using JudgeSystem.Web.ViewModels.User;

using Moq;
using Xunit;

namespace JudgeSystem.Services.Data.Tests
{
    public class UserServiceTests : TransientDbContextProvider
    {
        [Fact]
        public void GetContestResults_WithValidData_ShouldWorkCorerct()
        {
            List<ApplicationUser> testData = GetContestResultsTestData();
            UserService service = CreateUserServiceWithMockedRepository(testData.AsQueryable());

            List<UserCompeteResultViewModel> actualResults = service.GetContestResults("test_user");
            var expectedResult = testData.First().UserContests.Select(s => s.Contest).ToList();

            Assert.Equal(2, actualResults.Count);
            UserCompeteResultViewModel firstResult = actualResults[0];
            UserCompeteResultViewModel secondReslut = actualResults[1];
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
            List<ApplicationUser> testData = GetPracticeResultsTestData();
            UserService service = CreateUserServiceWithMockedRepository(testData.AsQueryable());

            List<UserPracticeResultViewModel> actualResults = service.GetPracticeResults("test_user");
            var expectedResult = testData.First().UserPractices.Select(s => s.Practice).ToList();

            Assert.Equal(2, actualResults.Count);
            UserPracticeResultViewModel firstResult = actualResults[0];
            UserPracticeResultViewModel secondReslut = actualResults[1];
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
            List<ApplicationUser> testData = GetUserExamResultsTestData();
            UserService service = CreateUserServiceWithMockedRepository(testData.AsQueryable());

            var actualResult = service.GetUserExamResults("test_user").ToList();

            Assert.Equal(2, actualResult.Count);
            UserCompeteResultViewModel firstExamResult = actualResult[0];
            UserCompeteResultViewModel secondExamResult = actualResult[1];
            Assert.Equal(180, firstExamResult.ActualPoints);
            Assert.Equal(200, firstExamResult.MaxPoints);
            Assert.Equal(1, firstExamResult.LessonId);
            Assert.Equal("Programming Basics", firstExamResult.ContestName);
            Assert.Equal(250, secondExamResult.ActualPoints);
            Assert.Equal(300, secondExamResult.MaxPoints);
            Assert.Equal(3, secondExamResult.LessonId);
            Assert.Equal("Exam contest", secondExamResult.ContestName);
        }

        [Theory]
        [InlineData("student_id")]
        [InlineData(null)]
        public async Task DeleteUserData_WiithExistingUserId_ShouldDeleteAllUserDataInTheDatabase(string studentId)
        {
            ApplicationUser user = GetUser();
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            IRepository<UserContest> userContestRepository = new EfRepository<UserContest>(context);
            IRepository<UserPractice> userPracticeRepository = new EfRepository<UserPractice>(context);
            var studentServiceMock = new Mock<IStudentService>();
            studentServiceMock.Setup(x => x.Delete(studentId));
            var userService = new UserService(null, userPracticeRepository, userContestRepository, studentServiceMock.Object);

            await userService.DeleteUserData(user.Id, studentId);

            Assert.Equal(0, context.UserContests.Count(x => x.UserId == user.Id));
            Assert.Equal(0, context.UserPractices.Count(x => x.UserId == user.Id));
            if(studentId != null)
            {
                studentServiceMock.Verify(x => x.Delete(studentId), Times.Once());
            }
            else
            {
                studentServiceMock.Verify(x => x.Delete(studentId), Times.Never());
            }
        }

        private UserService CreateUserServiceWithMockedRepository(IQueryable<ApplicationUser> testData)
        {
            var reposotiryMock = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            reposotiryMock.Setup(x => x.All()).Returns(testData);
            return new UserService(reposotiryMock.Object, null, null, null);
        }

        private List<ApplicationUser> GetContestResultsTestData()
        {
            var sumTwoNumbers = new Problem { Id = 1, Name = "Sum two numbers", IsExtraTask = true, MaxPoints = 100 };
            var helloWorld = new Problem { Id = 2, Name = "Hello world", IsExtraTask = false, MaxPoints = 100 };
            var multiply = new Problem { Id = 3, Name = "Multiply", IsExtraTask = false, MaxPoints = 100 };
            var interfaces = new Problem { Id = 4, Name = "Interfaces", IsExtraTask = false, MaxPoints = 100 };
            var inheritance = new Problem { Id = 5, Name = "Inheritance", IsExtraTask = false, MaxPoints = 100 };

            var oopLesson = new Lesson
            {
                Id = 2,
                Name = "OOP",
                Problems = new List<Problem> { interfaces, inheritance }
            };
            var cSharpLesson = new Lesson
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
            var sumTwoNumbers = new Problem { Id = 1, Name = "Sum two numbers", IsExtraTask = true, MaxPoints = 100 };
            var helloWorld = new Problem { Id = 2, Name = "Hello world", IsExtraTask = false, MaxPoints = 100 };
            var multiply = new Problem { Id = 3, Name = "Multiply", IsExtraTask = false, MaxPoints = 100 };
            var interfaces = new Problem { Id = 4, Name = "Interfaces", IsExtraTask = false, MaxPoints = 100 };
            var inheritance = new Problem { Id = 5, Name = "Inheritance", IsExtraTask = false, MaxPoints = 100 };

            var oopLesson = new Lesson
            {
                Id = 2,
                Name = "OOP",
                Problems = new List<Problem> { interfaces, inheritance }
            };
            var cSharpLesson = new Lesson
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
            var sumTwoNumbers = new Problem { Id = 1, Name = "Sum two numbers", IsExtraTask = true, MaxPoints = 100 };
            var helloWorld = new Problem { Id = 2, Name = "Hello world", IsExtraTask = false, MaxPoints = 100 };
            var multiply = new Problem { Id = 3, Name = "Multiply", IsExtraTask = false, MaxPoints = 100 };
            var interfaces = new Problem { Id = 4, Name = "Interfaces", IsExtraTask = false, MaxPoints = 100 };
            var inheritance = new Problem { Id = 5, Name = "Inheritance", IsExtraTask = false, MaxPoints = 100 };
            var examProblem = new Problem { Id = 6, Name = "Inheritance", IsExtraTask = false, MaxPoints = 300 };

            var oopLesson = new Lesson
            {
                Id = 2,
                Name = "OOP",
                Problems = new List<Problem> { interfaces, inheritance },
                Type = LessonType.Exercise
            };
            var cSharpLesson = new Lesson
            {
                Id = 1,
                Name = "C#",
                Problems = new List<Problem> { sumTwoNumbers, helloWorld, multiply },
                Type = LessonType.Exam
            };
            var examLesson = new Lesson
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

        private ApplicationUser GetUser()
        {
            var user = new ApplicationUser
            {
                Id = "user_id",
                Student = new Student() { Id = "student_id" },
                UserPractices = new List<UserPractice>
                {
                    new UserPractice
                    {
                        Practice = new Practice() { Id = 1 }
                    },
                    new UserPractice
                    {
                        Practice = new Practice() { Id = 2 }
                    },
                },
                UserContests = new List<UserContest>
                {
                    new UserContest()
                    {
                        Contest = new Contest(){ Id = 11 },
                    },
                    new UserContest()
                    {
                        Contest = new Contest(){ Id = 12 },
                    }
                }
            };

            return user;
        }
    }
}
