using System.Linq;

using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Web.Areas.Administration.Controllers;
using JudgeSystem.Web.InputModels.Course;
using JudgeSystem.Web.Tests.TestData;
using JudgeSystem.Web.ViewModels.Course;

using MyTested.AspNetCore.Mvc;
using Xunit;

namespace JudgeSystem.Web.Tests.Administration.Controllers
{
    public class CourseControllerTests
    {
        [Fact]
        public void Create_ShouldReturnView() =>
          MyController<CourseController>
          .Instance()
          .Calling(c => c.Create())
          .ShouldReturn()
          .View();

        [Fact]
        public void Create_ShouldHaveAttribtesForPostRequestAndAntiForgeryToken() =>
            MyController<CourseController>
            .Instance()
            .Calling(c => c.Create(With.Default<CourseInputModel>()))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .ValidatingAntiForgeryToken());

        [Theory]
        [InlineData(TestConstnts.StringLessThan3Symbols)]
        [InlineData(TestConstnts.StringMoreThan50Symbols)]
        public void Create_WithInvalidInputData_ShouldHaveInvalidModelStateAndReturnViewWithTheSameViewModel(string name) =>
            MyController<CourseController>
            .Instance()
            .Calling(c => c.Create(new CourseInputModel { Name = name }))
            .ShouldHave()
            .InvalidModelState()
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<CourseInputModel>()
                .Passing(model => model.Name == name));

        [Fact]
        public void Create_WithValidInputData_ShouldReturnRedirectResultAndShoudAddTheCourseInTheDb()
        {
            var inputModel = new CourseInputModel { Name = "C# OOP" };

            MyController<CourseController>
            .Instance()
            .Calling(c => c.Create(inputModel))
            .ShouldHave()
            .Data(data => data
                .WithSet<Course>(set =>
                {
                    Assert.NotNull(set.FirstOrDefault(f => f.Name == inputModel.Name));
                }))
            .AndAlso()
            .ShouldReturn()
            .Redirect(result => result.To<Web.Controllers.CourseController>(c => c.All()));
        }

        [Fact]
        public void Edit_WithValidId_ShoudReturnViewWithCorrectModel()
        {
            Course course = CourseTestData.GetEntity();

            MyController<CourseController>
            .Instance()
            .WithData(course)
            .Calling(c => c.Edit(course.Id))
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<CourseEditModel>()
                .Passing(model =>
                {
                    Assert.Equal(course.Id, model.Id);
                    Assert.Equal(course.Name, model.Name);
                }));
        }

        [Fact]
        public void Edit_ShouldHaveAttribtesForPostRequestAndAntiForgeryToken() =>
            MyController<CourseController>
            .Instance()
            .Calling(c => c.Edit(With.Default<CourseEditModel>()))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .ValidatingAntiForgeryToken());

        [Theory]
        [InlineData(TestConstnts.StringLessThan3Symbols)]
        [InlineData(TestConstnts.StringMoreThan50Symbols)]
        public void Edit_WithInvlidInputData_ShouldHaveInvalidModelStateAndReturnViewWithTheSameViewModel(string name) =>
            MyController<CourseController>
            .Instance()
            .Calling(c => c.Edit(new CourseEditModel { Name = name }))
            .ShouldHave()
            .InvalidModelState()
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<CourseEditModel>()
                .Passing(model => model.Name == name));

        [Fact]
        public void Edit_WithValidInputData_ShouldReturnRedirectResultAndShouldUpdateTheCourse()
        {
            Course course = CourseTestData.GetEntity();
            var inputModel = new CourseEditModel
            {
                Id = course.Id,
                Name = "C# OOP edited"
            };

            MyController<CourseController>
            .Instance()
            .WithData(course)
            .Calling(c => c.Edit(inputModel))
            .ShouldHave()
            .Data(data => data
                .WithSet<Course>(set =>
                {
                    Course editedCourse = set.FirstOrDefault(c => c.Id == inputModel.Id);
                    Assert.NotNull(editedCourse);
                    Assert.Equal(inputModel.Name, editedCourse.Name);
                }))
            .AndAlso()
            .ShouldReturn()
            .Redirect(result => result.To<Web.Controllers.CourseController>(a => a.All()));
        }

        [Fact]
        public void Delete_WithValidId_ShoudReturnViewWithCorrectModel()
        {
            Course course = CourseTestData.GetEntity();

            MyController<CourseController>
            .Instance()
            .WithData(course)
            .Calling(c => c.Delete(course.Id))
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<CourseViewModel>()
                .Passing(model =>
                {
                    Assert.Equal(course.Id, model.Id);
                    Assert.Equal(course.Name, model.Name);
                }));
        }

        [Fact]
        public void DeleteConfirm_ShouldHaveAttribtesForPostRequestAntiForgeryTokenAndActionName() =>
            MyController<CourseController>
            .Instance()
            .WithData(CourseTestData.GetEntity())
            .Calling(c => c.DeleteConfirm(1))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .ValidatingAntiForgeryToken()
                .ChangingActionNameTo(nameof(CourseController.Delete)));

        [Fact]
        public void DeleteConfirm_WithCourseWithTheSameIdInTheDb_ShouldDeleteTheCourseAndReturnRedirectResult()
        {
            Course course = CourseTestData.GetEntity();

            MyController<CourseController>
           .Instance()
           .WithData(course)
           .Calling(c => c.DeleteConfirm(course.Id))
           .ShouldHave()
           .Data(data => data
                .WithSet<Course>(set =>
                {
                    bool courseExists = set.Any(c => c.Id == course.Id);
                    Assert.False(courseExists);
                }))
            .AndAlso()
            .ShouldReturn()
            .Redirect(result => result.To<Web.Controllers.CourseController>(a => a.All()));
        }
    }
}
