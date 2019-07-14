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

namespace JudgeSystem.Services.Data.Tests
{
    public class LessonServiceTests : TransientDbContextProvider
    {
        [Fact]
        public async Task CreateLesson_WithValidData_ShouldWorkCorrect()
        {
            var lessonInputModel = new LessonInputModel() { Name = "test", CourseId = 5 };
            var resources = new List<Resource>()
            {
                new Resource { Name = "resource1", },
                new Resource { Name = "resource2", },
                new Resource { Name = "resource3", },
            };

            IDeletableEntityRepository<Lesson> repository = new EfDeletableEntityRepository<Lesson>(this.context);
            var lessonService = new LessonService(repository);
            await lessonService.CreateLesson(lessonInputModel, resources);
            Assert.True(context.Lessons.Any(l => l.Name == "test" && l.CourseId == 5));
            Assert.True(context.Resources.Count() == 3);
        }

        [Fact]
        public void CourseLessonsByType_WithValidData_ShouldReturnCorrectData()
        {
            var testData = GetTestData();
            var service = CreateLessonServiceWithMockedRepository(testData.AsQueryable());
            var actualLesosns = service.CourseLessonsByType("Exam", 10);

            Assert.Equal(2, actualLesosns.Count());
            Assert.True(actualLesosns.All(x =>x.CourseId == 10 && x.Type == LessonType.Exam));
        }

        [Fact]
        public void CourseLessonsByType_WithInValidLessonType_ShouldReturnEmptyCollection()
        {
            var service = CreateLessonServiceWithMockedRepository(new List<Lesson>().AsQueryable());
            var actualLesosns = service.CourseLessonsByType("Exam", 10);
            Assert.Empty(actualLesosns);
        }

        private LessonService CreateLessonServiceWithMockedRepository(IQueryable<Lesson> testData)
        {
            var reposotiryMock = new Mock<IDeletableEntityRepository<Lesson>>();
            reposotiryMock.Setup(x => x.All()).Returns(testData);
            return new LessonService(reposotiryMock.Object);
        }

        [Fact]
        public async Task Delete_WithValidData_ShouldWorkCorrect()
        {
            var testData = GetTestData();
            await this.context.Lessons.AddRangeAsync(testData);
            await this.context.SaveChangesAsync();
            IDeletableEntityRepository<Lesson> repository = new EfDeletableEntityRepository<Lesson>(this.context);
            var lessonService = new LessonService(repository);
            var lesson = testData.First(x => x.Name == "test2");
            await lessonService.Delete(lesson);
            Assert.False(this.context.Lessons.Any(x => x.Name == lesson.Name));
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
    }
}
