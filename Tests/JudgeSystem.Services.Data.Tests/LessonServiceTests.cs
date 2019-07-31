using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Web.InputModels.Lesson;
using System.Collections.Generic;
using System.Threading.Tasks;
using JudgeSystem.Data.Repositories;
using Xunit;
using System.Linq;
using JudgeSystem.Data.Models.Enums;
using Moq;
using System;
using JudgeSystem.Common;
using JudgeSystem.Web.Infrastructure.Exceptions;

namespace JudgeSystem.Services.Data.Tests
{
    public class LessonServiceTests : TransientDbContextProvider
    {
        [Fact]
        public async Task CreateLesson_WithValidData_ShouldWorkCorrect()
        {
            var lessonInputModel = new LessonInputModel() { Name = "test", CourseId = 5 };
            IDeletableEntityRepository<Lesson> repository = new EfDeletableEntityRepository<Lesson>(this.context);
            var lessonService = new LessonService(repository);

            await lessonService.CreateLesson(lessonInputModel);

            Assert.True(context.Lessons.Any(l => l.Name == "test" && l.CourseId == 5));
        }

        [Fact]
        public void CourseLessonsByType_WithValidData_ShouldReturnCorrectData()
        {
            var testData = GetTestData();
            var service = CreateLessonServiceWithMockedRepository(testData.AsQueryable());

            var actualLesosns = service.CourseLessonsByType("Exam", 10);

            Assert.Equal(2, actualLesosns.Count());
            Assert.True(actualLesosns.All(x => x.CourseId == 10 && x.Type == LessonType.Exam));
        }

        [Fact]
        public void CourseLessonsByType_WithInValidLessonType_ShouldReturnEmptyCollection()
        {
            var service = CreateLessonServiceWithMockedRepository(new List<Lesson>().AsQueryable());

            var actualLesosns = service.CourseLessonsByType("Exam", 10);

            Assert.Empty(actualLesosns);
        }

        [Fact]
        public async Task Delete_WithValidData_ShouldWorkCorrect()
        {
            var testData = GetTestData();
            var lessonService = await CreateLessonService(testData);

            var lesson = testData.First(x => x.Name == "test2");
            await lessonService.Delete(lesson);

            Assert.False(this.context.Lessons.Any(x => x.Name == lesson.Name));
        }

        [Fact]
        public async Task Delete_WithNonExistingLesson_ShouldThrowError()
        {
            IDeletableEntityRepository<Lesson> repository = new EfDeletableEntityRepository<Lesson>(this.context);
            var lessonService = new LessonService(repository);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => lessonService.Delete(new Lesson { Id = 161651 }));
        }

        [Fact]
        public async Task GetById_WithValidId_ShouldReturnCorrectData()
        {
            var testData = GetTestWithIds();
            var service = await CreateLessonService(testData);

            int id = 2;
            var actualData = await service.GetById(id);
            var expectedData = testData[id - 1];

            Assert.Equal(actualData.Name, expectedData.Name);
            Assert.Equal(actualData.Type, expectedData.Type);
            Assert.Equal(actualData.CourseId, expectedData.CourseId);
        }

        [Fact]
        public async Task GetById_WithInValidId_ShouldReturnThrowEntityNotFoundException()
        {
            var testData = GetTestData();
            LessonService service = await CreateLessonService(testData);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetById(161651));
        }

        [Theory]
        [InlineData(10, LessonType.Exam, "test1, test5")]
        [InlineData(1, LessonType.Homework, "test3")]
        [InlineData(10, LessonType.Homework, "")]
        [InlineData(45, LessonType.Exercise, "")]
        public async Task GetCourseLesosns_WithValidData_ShouldWorkCorrect(int courseId, LessonType lessonType,
            string expectedLessonsNames)
        {
            var testData = GetTestData();
            var service = await CreateLessonService(testData);

            var actualData = service.GetCourseLesosns(courseId, lessonType);
            string actualLessonsNames = string.Join(", ", actualData.Select(x => x.Name));

            Assert.Equal(expectedLessonsNames, actualLessonsNames);
        }

        [Fact]
        public async Task GetLessonInfo_WithValidId_ShouldReturnValidData()
        {
            var testData = GetTestWithIds();
            var service = await CreateLessonService(testData);
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
                }
            };
            await context.AddAsync(expectedLesson);
            await context.SaveChangesAsync();

            var actualLesson = await service.GetLessonInfo(999);

            Assert.Equal(expectedLesson.Name, actualLesson.Name);
            Assert.Equal(expectedLesson.CourseId, actualLesson.CourseId);
            Assert.Equal(expectedLesson.Problems.Count, actualLesson.Problems.Count);
            Assert.Equal(expectedLesson.Resources.Count, actualLesson.Resources.Count);
            Assert.Equal(expectedLesson.Resources.Select(x => x.Name).ToList(), actualLesson.Resources.Select(x => x.Name).ToList());
            Assert.Equal(expectedLesson.Problems.Select(x => x.Name).ToList(), actualLesson.Problems.Select(x => x.Name).ToList());
        }

        [Fact]
        public async Task GetLessonInfo()
        {
            var testData = GetTestData();
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
            var service = await CreateLessonService(GetTestData());
            await context.AddRangeAsync(new List<Lesson>
            {
                new Lesson { Name = "programming basics" },
                new Lesson { Name = "c# WEb API" },
                new Lesson { Name = "c# web - asp.NET CoRe" },
            });
            await context.SaveChangesAsync();

            var actualResult = service.SearchByName(keyword);

            Assert.Equal(expectedResult, string.Join(", ", actualResult.Select(x => x.Name)));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task SearchByName_WithDifferentInvalidInput_ShouldThrowArgumentException(string keyword)
        {
            var service = await CreateLessonService(GetTestData());
            await context.SaveChangesAsync();

            var exception = Assert.Throws<ArgumentException>(() => service.SearchByName(keyword));
            Assert.Equal(exception.Message, ErrorMessages.InvalidSearchKeyword);
        }

        [Fact]
        public async Task Update_WithValidData_ShouldWorkCorrect()
        {
            var testData = GetTestData();
            var lessonService = await CreateLessonService(testData);

            var lesson = context.Lessons.First(x => x.Name == "test2");
            lesson.Name = "edited";
            lesson.CourseId = 45;
            lesson.LessonPassword = "123456";
            lesson.Resources = new List<Resource> { new Resource { Name = "test"} };
            await lessonService.Update(lesson);
            var actualLesson = context.Lessons.First(x => x.Id == lesson.Id);

            Assert.Equal("edited", actualLesson.Name);
            Assert.Equal(45, actualLesson.CourseId);
            Assert.Equal(LessonType.Homework, actualLesson.Type);
            Assert.Equal("123456", actualLesson.LessonPassword);
            Assert.Equal(1, actualLesson.Resources.Count);
            Assert.Equal("test", actualLesson.Resources.First().Name);
        }

        [Fact]
        public async Task Update_WithNonExistingLesson_ShouldThrowError()
        {
            IDeletableEntityRepository<Lesson> repository = new EfDeletableEntityRepository<Lesson>(this.context);
            var lessonService = new LessonService(repository);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => lessonService.Update(new Lesson { Id = 161651 }));
        }

        private async Task<LessonService> CreateLessonService(List<Lesson> testData)
        {
            await this.context.Lessons.AddRangeAsync(testData);
            await this.context.SaveChangesAsync();
            IDeletableEntityRepository<Lesson> repository = new EfDeletableEntityRepository<Lesson>(this.context);
            var service = new LessonService(repository);
            return service;
        }

        private LessonService CreateLessonServiceWithMockedRepository(IQueryable<Lesson> testData)
        {
            var reposotiryMock = new Mock<IDeletableEntityRepository<Lesson>>();
            reposotiryMock.Setup(x => x.All()).Returns(testData);
            return new LessonService(reposotiryMock.Object);

        }

        private List<Lesson> GetTestData()
        {
            return new List<Lesson>()
            {
                new Lesson { Name = "test1", CourseId = 10, Type = LessonType.Exam},
                new Lesson { Name = "test2", CourseId = 8, Type = LessonType.Homework },
                new Lesson { Name = "test3", CourseId = 1, Type = LessonType.Homework },
                new Lesson { Name = "test4", CourseId = 10, Type = LessonType.Exercise },
                new Lesson { Name = "test5", CourseId = 10, Type = LessonType.Exam }
            };
        }

        private List<Lesson> GetTestWithIds()
        {
            return new List<Lesson>()
            {
                new Lesson { Id = 1, Name = "test1", CourseId = 10, Type = LessonType.Exam},
                new Lesson { Id = 2, Name = "test2", CourseId = 8, Type = LessonType.Homework },
                new Lesson { Id = 3, Name = "test3", CourseId = 1, Type = LessonType.Homework },
            };
        }
    }
}
