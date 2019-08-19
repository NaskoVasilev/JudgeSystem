using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using JudgeSystem.Common.Exceptions;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Repositories;
using JudgeSystem.Web.InputModels.Course;
using JudgeSystem.Web.ViewModels.Course;

using Moq;
using Xunit;

namespace JudgeSystem.Services.Data.Tests
{
    public class CourseServiceTests : TransientDbContextProvider
    {
        [Fact]
        public async Task Add_WithValidData_ShouldWorkCorrect()
        {
            CourseService courseService = await CreateCourseService(new List<Course>());
            string name = "Ef core and unit testing";
            var course = new CourseInputModel { Name = name };

            await courseService.Add(course);

            Assert.Equal(context.Courses.First().Name, name);
            Assert.True(context.Courses.First().Id != 0);
        }

        [Fact]
        public void All_With2Course_ShouldReturn2Courses()
        {
            List<Course> testData = GetTestData();
            CourseService courseService = CreateCourseServiceWithMockedRepository(testData.AsQueryable());

            IEnumerable<CourseViewModel> actualData = courseService.All();

            Assert.Equal(testData.Count, actualData.Count());
            foreach (CourseViewModel data in actualData)
            {
                Assert.Contains(testData, d => d.Id == data.Id && d.Name == data.Name);
            }
        }

        [Fact]
        public void All_WithNoData_ShouldReturnEmptyCollection()
        {
            ICourseService courseService = CreateCourseServiceWithMockedRepository(Enumerable.Empty<Course>().AsQueryable());

            IEnumerable<CourseViewModel> actualData = courseService.All();

            Assert.Empty(actualData);
        }

        [Fact]
        public void GetName_WithValidDataAndValidId_ShouldReturnCorrectName()
        {
            CourseService courseService = CreateCourseServiceWithMockedRepository(GetTestData().AsQueryable());

            string actualName = courseService.GetName(2);

            Assert.Equal("course2", actualName);
        }

        [Fact]
        public void GetName_WithValidDataAndInvalidId_ShouldReturnNull()
        {
            CourseService courseService = CreateCourseServiceWithMockedRepository(GetTestData().AsQueryable());

            Assert.Null(courseService.GetName(5));
        }

        [Fact]
        public void GetName_WithEmptyDatabase_ShouldReturnNull()
        {
            CourseService courseService = CreateCourseServiceWithMockedRepository(Enumerable.Empty<Course>().AsQueryable());

            Assert.Null(courseService.GetName(5));
        }

        [Fact]
        public async Task GetById_WithValidDataAndValidId_ShouldReturnCorrectResult()
        {
            CourseService courseService = await CreateCourseService(GetTestData());

            CourseEditModel actualCourse = courseService.GetById<CourseEditModel>(2);

            Assert.Equal("course2", actualCourse.Name);
            Assert.Equal(2, actualCourse.Id);
        }

        [Fact]
        public async Task GetById_WithInvalidId_ShouldThrowArgumentException()
        {
            CourseService courseService = await CreateCourseService(GetTestData());

            Assert.Throws<EntityNotFoundException>(() => courseService.GetById<CourseEditModel>(456));
        }

        [Fact]
        public async Task Update_WithValidData_ShouldWorkCorrect ()
        {
            CourseService courseService = await CreateCourseService(GetTestData());

            await courseService.Updade(new CourseEditModel { Id = 1, Name = "edited" });
            Course editedCourse = context.Courses.Find(1);

            Assert.Equal("edited", editedCourse.Name);
        }

        [Fact]
        public async Task Update_WithInvalidId_ShouldThrowArgumentException()
        {
            CourseService courseService = await CreateCourseService(GetTestData());

            await Assert.ThrowsAsync<EntityNotFoundException>(() => courseService
            .Updade(new CourseEditModel { Id = 5, Name = "fakeEdited" }));
        }

        [Fact]
        public async Task Delete_WithValidData_ShouldWorkCorrect()
        {
            CourseService courseService = await CreateCourseService(GetTestData());

            await courseService.Delete(2);

            Assert.False(context.Courses.Any(x => x.Id == 2));
        }

        [Fact]
        public async Task Delete_WithInvalidId_ShouldThrowArgumentException()
        {
            CourseService courseService = await CreateCourseService(GetTestData());

            await Assert.ThrowsAsync<EntityNotFoundException>(() => courseService.Delete(45));
        }

        private async Task<CourseService> CreateCourseService(List<Course> testData)
        {
            await context.Courses.AddRangeAsync(testData);
            await context.SaveChangesAsync();
            IDeletableEntityRepository<Course> repository = new EfDeletableEntityRepository<Course>(context);
            var service = new CourseService(repository);
            return service;
        }

        private CourseService CreateCourseServiceWithMockedRepository(IQueryable<Course> testData)
        {
            var reposotiryMock = new Mock<IDeletableEntityRepository<Course>>();
            reposotiryMock.Setup(x => x.All()).Returns(testData);
            return new CourseService(reposotiryMock.Object);
        }

        private List<Course> GetTestData()
        {
            var courses = new List<Course>()
            {
                new Course{ Name = "course1", Id = 1 },
                new Course{ Name = "coirse3", Id = 3 },
                new Course{ Name = "course2", Id = 2 }
            };
            return courses;
        }
    }
}
