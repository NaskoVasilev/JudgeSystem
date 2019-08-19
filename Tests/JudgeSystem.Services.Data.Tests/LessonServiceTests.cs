using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Web.InputModels.Lesson;
using JudgeSystem.Data.Repositories;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Common;
using JudgeSystem.Common.Exceptions;
using JudgeSystem.Web.ViewModels.Lesson;
using JudgeSystem.Web.Dtos.Lesson;
using JudgeSystem.Web.ViewModels.Search;

using Xunit;
using Moq;

namespace JudgeSystem.Services.Data.Tests
{
    public class LessonServiceTests : TransientDbContextProvider
    {
        private readonly IPasswordHashService hashService = new PasswordHashService();

        [Fact]
        public async Task CreateLesson_WithValidData_ShouldWorkCorrect()
        {
            var lessonInputModel = new LessonInputModel() { Name = "test", CourseId = 5 };
            IDeletableEntityRepository<Lesson> repository = new EfDeletableEntityRepository<Lesson>(context);
            var lessonService = new LessonService(repository, hashService);

            await lessonService.Create(lessonInputModel);

            Assert.True(context.Lessons.Any(l => l.Name == "test" && l.CourseId == 5));
        }

        [Fact]
        public void CourseLessonsByType_WithValidData_ShouldReturnCorrectData()
        {
            List<Lesson> testData = GetTestData();
            LessonService service = CreateLessonServiceWithMockedRepository(testData.AsQueryable());

            IEnumerable<LessonLinkViewModel> actualLesosns = service.CourseLessonsByType("Exam", 10);

            Assert.Equal(2, actualLesosns.Count());
            Assert.True(actualLesosns.All(x => x.CourseId == 10 && x.Type == LessonType.Exam));
        }

        [Fact]
        public void CourseLessonsByType_WithInValidLessonType_ShouldReturnEmptyCollection()
        {
            LessonService service = CreateLessonServiceWithMockedRepository(new List<Lesson>().AsQueryable());

            IEnumerable<LessonLinkViewModel> actualLesosns = service.CourseLessonsByType("Exam", 10);

            Assert.Empty(actualLesosns);
        }

        [Fact]
        public async Task Delete_WithValidData_ShouldWorkCorrect()
        {
            List<Lesson> testData = GetTestData();
            LessonService lessonService = await CreateLessonService(testData);

            Lesson lesson = testData.First(x => x.Name == "test2");
            await lessonService.Delete(lesson.Id);

            Assert.False(context.Lessons.Any(x => x.Name == lesson.Name));
        }

        [Fact]
        public async Task Delete_WithNonExistingLesson_ShouldThrowError()
        {
            IDeletableEntityRepository<Lesson> repository = new EfDeletableEntityRepository<Lesson>(context);
            var lessonService = new LessonService(repository, hashService);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => lessonService.Delete(44894));
        }

        [Fact]
        public async Task GetById_WithValidId_ShouldReturnCorrectData()
        {
            List<Lesson> testData = GetTestWithIds();
            LessonService service = await CreateLessonService(testData);

            int id = 2;
            LessonEditInputModel actualData = await service.GetById<LessonEditInputModel>(id);
            Lesson expectedData = testData[id - 1];

            Assert.Equal(actualData.Name, expectedData.Name);
            Assert.Equal(actualData.Type, expectedData.Type);
        }

        [Fact]
        public async Task GetById_WithInValidId_ShouldThrowEntityNotFoundException()
        {
            List<Lesson> testData = GetTestData();
            LessonService service = await CreateLessonService(testData);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetById<LessonEditInputModel>(161651));
        }

        [Theory]
        [InlineData(10, LessonType.Exam, "test1, test5")]
        [InlineData(1, LessonType.Homework, "test3")]
        [InlineData(10, LessonType.Homework, "")]
        [InlineData(45, LessonType.Exercise, "")]
        public async Task GetCourseLesosns_WithValidData_ShouldWorkCorrect(int courseId, LessonType lessonType,
            string expectedLessonsNames)
        {
            List<Lesson> testData = GetTestData();
            LessonService service = await CreateLessonService(testData);

            IEnumerable<ContestLessonDto> actualData = service.GetCourseLesosns(courseId, lessonType);
            string actualLessonsNames = string.Join(", ", actualData.Select(x => x.Name));

            Assert.Equal(expectedLessonsNames, actualLessonsNames);
        }

        [Fact]
        public async Task GetFirstProblemId_WithLessonWithProblems_ShouldReturnCorrectData()
        {
            var lesson = new Lesson
            {
                Id = 10,
                Problems = new List<Problem>
                {
                    new Problem { Id = 12 },
                    new Problem { Id = 15 },
                }

            };
            LessonService service = await CreateLessonService(new List<Lesson> { lesson });

            int? actualData = service.GetFirstProblemId(lesson.Id);

            Assert.Equal(12, actualData);
        }

        [Fact]
        public async Task GetFirstProblemId_WithInvalidLessonId_ShouldThrowEntityNotFoundException()
        {
            List<Lesson> testData = GetTestData();
            LessonService service = await CreateLessonService(testData);

            Assert.Throws<EntityNotFoundException>(() => service.GetFirstProblemId(161651));
        }

        [Fact]
        public async Task GetFirstProblemId_WithLessonWithoutProblems_ShouldReturnNull()
        {
            var lesson = new Lesson { Id = 10 };
            LessonService service = await CreateLessonService(new List<Lesson> { lesson });

            int? actualData = service.GetFirstProblemId(lesson.Id);

            Assert.Null(actualData);
        }

        [Fact]
        public async Task GetPracticeId_WithValidId_ShouldReturnCorrectData()
        {
            var lesson = new Lesson
            {
                Id = 10,
                Practice = new Practice { Id = 20 }
            };
            LessonService service = await CreateLessonService(new List<Lesson> { lesson });

            int practiceId = service.GetPracticeId(lesson.Id);

            Assert.Equal(lesson.Practice.Id, practiceId);
        }

        [Fact]
        public async Task GetPracticeId_WithInValidId_ShouldThrowEntityNotFoundException()
        {
            List<Lesson> testData = GetTestData();
            LessonService service = await CreateLessonService(testData);

            Assert.Throws<EntityNotFoundException>(() => service.GetPracticeId(161651));
        }

        [Fact]
        public async Task GetLessonInfo_WithValidId_ShouldReturnValidData()
        {
            List<Lesson> testData = GetTestWithIds();
            LessonService service = await CreateLessonService(testData);
            var expectedLesson = new Lesson
            {
                Id = 999,
                Name = "test2",
                CourseId = 4,
                Problems = new List<Problem>()
                {
                    new Problem { Name = "problem1" },
                    new Problem { Name = "problem2" }
                },
                Resources = new List<Resource>
                {
                    new Resource { Name = "res1" },
                    new Resource { Name = "res2" },
                    new Resource { Name = "res3" }
                },
                Practice = new Practice { Id = 12 }
            };
            await context.AddAsync(expectedLesson);
            await context.SaveChangesAsync();

            LessonViewModel actualLesson = await service.GetLessonInfo(999);

            Assert.Equal(expectedLesson.Name, actualLesson.Name);
            Assert.Equal(expectedLesson.CourseId, actualLesson.CourseId);
            Assert.Equal(expectedLesson.Problems.Count, actualLesson.Problems.Count);
            Assert.Equal(expectedLesson.Resources.Count, actualLesson.Resources.Count);
            Assert.Equal(expectedLesson.Practice.Id, actualLesson.PracticeId);
            Assert.Equal(expectedLesson.Resources.Select(x => x.Name).ToList(), actualLesson.Resources.Select(x => x.Name).ToList());
            Assert.Equal(expectedLesson.Problems.Select(x => x.Name).ToList(), actualLesson.Problems.Select(x => x.Name).ToList());
        }

        [Fact]
        public async Task GetLessonInfo()
        {
            List<Lesson> testData = GetTestData();
            LessonService service = await CreateLessonService(testData);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetLessonInfo(161651));
        }

        [Theory]
        [InlineData("C#", "c# WEb API, c# web - asp.NET CoRe")]
        [InlineData("web", "c# WEb API, c# web - asp.NET CoRe")]
        [InlineData("test", "test1, test2, test3, test4, test5")]
        [InlineData("webasp", "")]
        [InlineData("asp", "c# web - asp.NET CoRe")]
        [InlineData("es", "test1, test2, test3, test4, test5")]
        [InlineData("csharp", "")]
        [InlineData("1", "test1")]
        public async Task SearchByName_WithDifferentInputs_ShouldWorkCorrect(string keyword, string expectedResult)
        {
            LessonService service = await CreateLessonService(GetTestData());
            await context.AddRangeAsync(new List<Lesson>
            {
                new Lesson { Name = "programming basics", Practice = new Practice() },
                new Lesson { Name = "c# WEb API", Practice = new Practice() },
                new Lesson { Name = "c# web - asp.NET CoRe", Practice = new Practice() },
            });
            await context.SaveChangesAsync();

            IEnumerable<SearchLessonViewModel> actualResult = service.SearchByName(keyword);

            Assert.Equal(expectedResult, string.Join(", ", actualResult.Select(x => x.Name)));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task SearchByName_WithDifferentInvalidInput_ShouldThrowBadRequestException(string keyword)
        {
            LessonService service = await CreateLessonService(GetTestData());
            await context.SaveChangesAsync();

            BadRequestException exception = Assert.Throws<BadRequestException>(() => service.SearchByName(keyword));
            Assert.Equal(exception.Message, ErrorMessages.InvalidSearchKeyword);
        }

        [Fact]
        public async Task Update_WithValidData_ShouldWorkCorrect()
        {
            List<Lesson> testData = GetTestData();
            LessonService lessonService = await CreateLessonService(testData);
            int id = context.Lessons.First().Id;
            var inputModel = new LessonEditInputModel
            {
                Id = id,
                Name = "Test123",
                Type = LessonType.Exercise
            };

            await lessonService.Update(inputModel);
            Lesson actualLesson = context.Lessons.First(x => x.Id == id);

            Assert.Equal(inputModel.Name, actualLesson.Name);
            Assert.Equal(inputModel.Type, actualLesson.Type);
        }

        [Fact]
        public async Task Update_WithNonExistingLesson_ShouldThrowError()
        {
            IDeletableEntityRepository<Lesson> repository = new EfDeletableEntityRepository<Lesson>(this.context);
            var lessonService = new LessonService(repository, hashService);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => lessonService.Update(new LessonEditInputModel { Id = 161651 }));
        }

        [Fact]
        public async Task SetPassword_WithInValidLessonId_ShouldThrowEntityNotFoundException()
        {
            List<Lesson> testData = GetTestData();
            LessonService lessonService = await CreateLessonService(testData);
            string lessonPassword = "test456456@$$";

            await Assert.ThrowsAsync<EntityNotFoundException>(() => lessonService.SetPassword(1231, lessonPassword));
        }

        [Fact]
        public async Task SetPassword_ToLessonWithAlreadyHasPassword_ShouldThrwowBadRequestException()
        {
            List<Lesson> testData = GetTestData();
            LessonService lessonService = await CreateLessonService(testData);
            int id = context.Lessons.First().Id;
            await AddLessonWithPassword();

            await Assert.ThrowsAsync<BadRequestException>(() => lessonService.SetPassword(9999, "dfjkdshjkf"));
        }

        [Fact]
        public async Task SetPassword_WithValidArguments_ShouldWorkCorrect()
        {
            List<Lesson> testData = GetTestData();
            LessonService lessonService = await CreateLessonService(testData);
            Lesson lesson = context.Lessons.First();
            string lessonPassword = "test456456@$$";

            await lessonService.SetPassword(lesson.Id, lessonPassword);

            Assert.Equal(hashService.HashPassword(lessonPassword), lesson.LessonPassword);
        }

        [Fact]
        public async Task UpdatePassword_WithInValidLessonId_ShouldThrowEntityNotFoundException()
        {
            List<Lesson> testData = GetTestData();
            LessonService lessonService = await CreateLessonService(testData);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => lessonService.UpdatePassword(1231, "sdsds", "sfdsfds"));
        }

        [Fact]
        public async Task UpdatePassword_WithDifferntOldPassword_ShouldThrowBadRequestException()
        {
            List<Lesson> testData = GetTestData();
            LessonService lessonService = await CreateLessonService(testData);
            await AddLessonWithPassword();

            await Assert.ThrowsAsync<BadRequestException>(() => lessonService.UpdatePassword(9999, "qwe123", "132456"));
        }

        [Fact]
        public async Task UpdatePassword_WithValidArguments_ShouldWorkCorrect()
        {
            List<Lesson> testData = GetTestData();
            LessonService lessonService = await CreateLessonService(testData);
            await AddLessonWithPassword();
            string newPassword = "123456QWE";

            await lessonService.UpdatePassword(9999, "123qwe", newPassword);

            Assert.Equal(hashService.HashPassword(newPassword), context.Lessons.First(x => x.Id == 9999).LessonPassword);
        }

        private async Task AddLessonWithPassword()
        {
            await context.Lessons.AddAsync(new Lesson { Id = 9999, Name = "test6", CourseId = 45,
                LessonPassword = hashService.HashPassword("123qwe"), Type = LessonType.Exercise });
            await context.SaveChangesAsync();
        }

        private async Task<LessonService> CreateLessonService(List<Lesson> testData)
        {
            await context.Lessons.AddRangeAsync(testData);
            await context.SaveChangesAsync();
            IDeletableEntityRepository<Lesson> repository = new EfDeletableEntityRepository<Lesson>(this.context);
            var service = new LessonService(repository, hashService);
            return service;
        }

        private LessonService CreateLessonServiceWithMockedRepository(IQueryable<Lesson> testData)
        {
            var reposotiryMock = new Mock<IDeletableEntityRepository<Lesson>>();
            reposotiryMock.Setup(x => x.All()).Returns(testData);
            return new LessonService(reposotiryMock.Object, hashService);
        }

        private List<Lesson> GetTestData()
        {
            var lessons = new List<Lesson>()
            {
                new Lesson { Name = "test1", CourseId = 10, Type = LessonType.Exam, Practice = new Practice() },
                new Lesson { Name = "test2", CourseId = 8, Type = LessonType.Homework, Practice = new Practice() },
                new Lesson { Name = "test3", CourseId = 1, Type = LessonType.Homework, Practice = new Practice() },
                new Lesson { Name = "test4", CourseId = 10, Type = LessonType.Exercise, Practice = new Practice() },
                new Lesson { Name = "test5", CourseId = 10, Type = LessonType.Exam, Practice = new Practice() }
            };
            return lessons;
        }

        private List<Lesson> GetTestWithIds()
        {
            var lessons = new List<Lesson>()
            {
                new Lesson { Id = 1, Name = "test1", CourseId = 10, Type = LessonType.Exam},
                new Lesson { Id = 2, Name = "test2", CourseId = 8, Type = LessonType.Homework },
                new Lesson { Id = 3, Name = "test3", CourseId = 1, Type = LessonType.Homework },
            };
            return lessons;
        }
    }
}
