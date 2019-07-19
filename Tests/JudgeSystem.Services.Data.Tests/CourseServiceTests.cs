using JudgeSystem.Common;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Repositories;
using JudgeSystem.Web.InputModels.Course;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace JudgeSystem.Services.Data.Tests
{
    public class CourseServiceTests : TransientDbContextProvider
    {
        [Fact]
        public async Task Add_WithValidData_ShouldWorkCorrect()
        {
            var courseService = CreateCourseService();
            string name = "Ef core and unit testing";
            var course = new CourseInputModel { Name = name };

            await courseService.Add(course);

            Assert.Equal(context.Courses.First().Name, name);
            Assert.True(context.Courses.First().Id != 0);
        }


        [Fact]
        public void All_With2Course_ShouldReturn2Courses()
        {
            var testData = GetTestData();
            var courseService = CreateCourseServiceWithMockdRepository();

            var actualData = courseService.All();

            Assert.Equal(testData.Count, actualData.Count());
            foreach (var data in actualData)
            {
                Assert.Contains(testData, d => d.Id == data.Id && d.Name == data.Name);
            }
        }

        [Fact]
        public void All_WithNoData_ShouldReturnEmptyCollection()
        {
            var repositoryMock = new Mock<IDeletableEntityRepository<Course>>();
            repositoryMock.Setup(r => r.All())
                .Returns(new List<Course>().AsQueryable());
            ICourseService courseService = new CourseService(repositoryMock.Object);

            var actualData = courseService.All();

            Assert.Empty(actualData);
        }

        [Fact]
        public void GetName_WithValidDataAndValidId_ShouldReturnCorrectName()
        {
            var courseService = CreateCourseServiceWithMockdRepository();

            var actualName = courseService.GetName(2);

            Assert.Equal("course2", actualName);
        }

        [Fact]
        public void GetName_WithValidDataAndInvalidId_ShouldReturnNull()
        {
            var courseService = CreateCourseServiceWithMockdRepository();

            Assert.Null(courseService.GetName(5));
        }

        [Fact]
        public void GetName_WithEmptyDatabase_ShouldReturnNull()
        {
            var repositoryMock = new Mock<IDeletableEntityRepository<Course>>();
            repositoryMock.Setup(r => r.All())
                .Returns(new List<Course>().AsQueryable());
            var courseService = new CourseService(repositoryMock.Object);

            Assert.Null(courseService.GetName(5));
        }

        [Fact]
        public async Task GetById_WithValidDataAndValidId_ShouldReturnCorrectResult()
        {
            context.AddRange(GetTestData());
            await context.SaveChangesAsync();
            var courseService = CreateCourseService();

            var actualCourse = await courseService.GetById(2);

            Assert.Equal("course2", actualCourse.Name);
            Assert.Equal(2, actualCourse.Id);
        }

        [Fact]
        public async Task GetById_WithValidDataAndInvalidId_ShouldReturnNull()
        {
            context.AddRange(GetTestData());
            await context.SaveChangesAsync();
            var courseService = CreateCourseService();

            Assert.Null(await courseService.GetById(5));
        }

        [Fact]
        public async Task GetById_WithEmptyDatabase_ShouldReturnNull()
        {
            var courseService = CreateCourseService();

            Assert.Null(await courseService.GetById(5));
        }

        [Fact]
        public async Task Update_WithValidData_ShouldWorkCorrect ()
        {
            context.AddRange(GetTestData());
            await context.SaveChangesAsync();
            var courseService = CreateCourseService();
            await courseService.Updade(new CourseEditModel { Id = 1, Name = "edited" });

            var editedCourse = await courseService.GetById(1);

            Assert.Equal("edited", editedCourse.Name);
        }

        [Fact]
        public async Task Update_WithInvalidId_ShouldThrowArgumentException()
        {
            context.AddRange(GetTestData());
            await context.SaveChangesAsync();
            var courseService = CreateCourseService();

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => courseService
            .Updade(new CourseEditModel { Id = 5, Name = "fakeEdited" }));

            Assert.Equal(exception.Message, string.Format(ErrorMessages.NotFoundEntityMessage, "course"));
        }

        [Fact]
        public async Task Delete_WithValidData_ShouldWorkCorrect()
        {
            context.AddRange(GetTestData());
            await context.SaveChangesAsync();
            var courseService = CreateCourseService();

            var course = await courseService.GetById(2);
            await courseService.Delete(course);

            Assert.True(course.IsDeleted);
        }

        private ICourseService CreateCourseServiceWithMockdRepository()
        {
            var testData = GetTestData();
            var repositoryMock = new Mock<IDeletableEntityRepository<Course>>();
            repositoryMock.Setup(r => r.All())
                .Returns(testData.AsQueryable());

            return new CourseService(repositoryMock.Object);
        }

        private ICourseService CreateCourseService()
        {
            IDeletableEntityRepository<Course> repository = new EfDeletableEntityRepository<Course>(this.context);
            return new CourseService(repository);
        }

        private List<Course> GetTestData()
        {
            return new List<Course>()
            {
                new Course{ Name = "course1", Id = 1 },
                new Course{ Name = "coirse3", Id = 3 },
                new Course{ Name = "course2", Id = 2 }
            };
        }
    }
}
