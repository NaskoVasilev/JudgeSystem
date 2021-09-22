using System.Linq;

using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Web.Areas.Administration.Controllers;
using JudgeSystem.Web.InputModels.Lesson;
using JudgeSystem.Web.Tests.TestData;
using JudgeSystem.Services;

using MyTested.AspNetCore.Mvc;
using Xunit;

namespace JudgeSystem.Web.Tests.Administration.Controllers
{
    public class LessonControllerTests
    {
        [Fact]
        public void Create_WithPassedLessonType_ShouldReturnViewWithModelWithSetLessonType() =>
          MyController<LessonController>
          .Instance()
          .Calling(c => c.Create(LessonType.Exam))
          .ShouldReturn()
          .View(result => result
                .WithModelOfType<LessonInputModel>()
                .Passing(model => model.Type == LessonType.Exam));

        [Fact]
        public void Create_ShouldHaveAttribtesForPostRequestAndAntiForgeryToken() =>
            MyController<LessonController>
            .Instance()
            .Calling(c => c.Create(With.Default<LessonInputModel>()))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .ValidatingAntiForgeryToken());

        [Theory]
        [InlineData(TestConstnts.StringLessThan3Symbols)]
        [InlineData(TestConstnts.StringMoreThan50Symbols)]
        public void Create_WithInvalidInputData_ShouldHaveInvalidModelStateAndReturnViewWithTheSameViewModel(string name)
        {
            var inputModel = new LessonInputModel
            {
                Name = name,
                Type = LessonType.Exam,
                CourseId = 1,
                LessonPassword = null
            };

            MyController<LessonController>
            .Instance()
            .Calling(c => c.Create(inputModel))
            .ShouldHave()
            .InvalidModelState()
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<LessonInputModel>()
                .Passing(model => model.Name == name && model.Type == inputModel.Type && model.CourseId == inputModel.CourseId));
        }

        [Fact]
        public void Create_WithValidInputData_ShouldReturnRedirectResultAndShoudAddTheLessonInTheDb()
        {
            Course course = CourseTestData.GetEntity();
            Lesson createdLesson = null;
            var inputModel = new LessonInputModel
            {
                Name = "Reflection",
                Type = LessonType.Exam,
                CourseId = course.Id,
                LessonPassword = null
            };

            MyController<LessonController>
            .Instance()
            .WithData(course)
            .Calling(c => c.Create(inputModel))
            .ShouldHave()
            .Data(data => data
                .WithSet<Lesson>(set =>
                {
                    createdLesson = set.FirstOrDefault(f => f.Name == inputModel.Name);
                    Assert.NotNull(createdLesson);
                    Assert.Equal(inputModel.CourseId, createdLesson.CourseId);
                    Assert.Equal(inputModel.Name, createdLesson.Name);
                    Assert.Equal(inputModel.Type, createdLesson.Type);

                    data.WithSet<Practice>(set => Assert.True(set.Any(p => p.LessonId == createdLesson.Id)));
                }))
            .AndAlso()
            .ShouldReturn()
            .Redirect(result => result.To<Web.Controllers.LessonController>(c => c.Details(createdLesson.Id, null, createdLesson.Practice.Id)));
        }

        [Fact]
        public void Edit_WithValidId_ShoudReturnViewWithCorrectModel()
        {
            Lesson lesson = LessonTestData.GetEntity();

            MyController<LessonController>
            .Instance()
            .WithData(lesson)
            .Calling(c => c.Edit(lesson.Id))
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<LessonEditInputModel>()
                .Passing(model =>
                {
                    Assert.Equal(lesson.Id, model.Id);
                    Assert.Equal(lesson.Name, model.Name);
                    Assert.Equal(lesson.Type, model.Type);
                }));
        }

        [Fact]
        public void Edit_ShouldHaveAttribtesForPostRequestAndAntiForgeryToken() =>
            MyController<LessonController>
            .Instance()
            .Calling(c => c.Edit(With.Default<LessonEditInputModel>()))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .ValidatingAntiForgeryToken());

        [Theory]
        [InlineData(TestConstnts.StringLessThan3Symbols)]
        [InlineData(TestConstnts.StringMoreThan50Symbols)]
        public void Edit_WithInvlidInputData_ShouldHaveInvalidModelStateAndReturnViewWithTheSameViewModel(string name) =>
            MyController<LessonController>
            .Instance()
            .Calling(c => c.Edit(new LessonEditInputModel { Name = name }))
            .ShouldHave()
            .InvalidModelState()
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<LessonEditInputModel>()
                .Passing(model => model.Name == name));

        [Fact]
        public void Edit_WithValidInputData_ShouldReturnRedirectResultAndShouldUpdateTheLesson()
        {
            Course course = CourseTestData.GetEntity();
            Lesson lesson = LessonTestData.GetEntity();
            var inputModel = new LessonEditInputModel
            {
                Id = lesson.Id,
                Name = "C# OOP edited",
                Type = LessonType.Exam
            };
            MyController<LessonController>
            .Instance()
            .WithData(lesson)
            .Calling(c => c.Edit(inputModel))
            .ShouldHave()
            .Data(data => data
                .WithSet<Lesson>(set =>
                {
                    Lesson editedLesson = set.FirstOrDefault(c => c.Id == inputModel.Id);
                    Assert.NotNull(editedLesson);
                    Assert.Equal(inputModel.Name, editedLesson.Name);
                    Assert.Equal(inputModel.Type, editedLesson.Type);
                }))
            .AndAlso()
            .ShouldReturn()
            .Redirect(result => result.To<Web.Controllers.CourseController>(a => a.Lessons(course.Id)));
        }

        [Fact]
        public void Delete_WithValidId_ShoudReturnViewWithCorrectModel()
        {
            Lesson lesson = LessonTestData.GetEntity();

            MyController<LessonController>
            .Instance()
            .WithData(lesson)
            .Calling(c => c.Delete(lesson.Id))
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<LessonEditInputModel>()
                .Passing(model =>
                {
                    Assert.Equal(lesson.Id, model.Id);
                    Assert.Equal(lesson.Name, model.Name);
                }));
        }

        [Fact]
        public void DeleteConfirm_ShouldHaveAttribtesForPostRequestAntiForgeryTokenAndActionName() =>
            MyController<LessonController>
            .Instance()
            .WithData(LessonTestData.GetEntity())
            .Calling(c => c.DeleteConfirm(1))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .ValidatingAntiForgeryToken()
                .ChangingActionNameTo(nameof(LessonController.Delete)));

        [Fact]
        public void DeleteConfirm_WithLessonWithTheSameIdInTheDb_ShouldDeleteTheLessonAndReturnRedirectResult()
        {
            Course course = CourseTestData.GetEntity();
            Lesson lesson = LessonTestData.GetEntity();

            MyController<LessonController>
           .Instance()
           .WithData(lesson)
           .Calling(c => c.DeleteConfirm(lesson.Id))
           .ShouldHave()
           .Data(data => data
                .WithSet<Lesson>(set =>
                {
                    bool lessonExists = set.Any(c => c.Id == lesson.Id);
                    Assert.False(lessonExists);
                }))
            .AndAlso()
            .ShouldReturn()
            .Redirect(result => result.To<Web.Controllers.CourseController>(c => c.Lessons(course.Id)));
        }

        [Fact]
        public void AddPassword_ShouldReturnView() =>
            MyController<LessonController>
            .Instance()
            .Calling(c => c.AddPassword())
            .ShouldReturn()
            .View();

        [Fact]
        public void AddPassword_ShouldHaveAttribtesForPostRequestAndAntiForgeryToken() =>
            MyController<LessonController>
            .Instance()
            .Calling(c => c.AddPassword(With.Default<LessonAddPasswordInputModel>()))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .ValidatingAntiForgeryToken());

        [Theory]
        [InlineData(TestConstnts.StringLessThan3Symbols)]
        [InlineData(TestConstnts.StringMoreThan50Symbols)]
        public void AddPassword_WithInvalidInputData_ShouldHaveInvalidModelStateAndReturnViewWithTheSameViewModel(string password)
        {
            Lesson lesson = LessonTestData.GetEntity();
            var inputModel = new LessonAddPasswordInputModel
            {
                Id = lesson.Id,
                LessonPassword = password
            };

            MyController<LessonController>
            .Instance()
            .WithData(lesson)
            .Calling(c => c.AddPassword(inputModel))
            .ShouldHave()
            .InvalidModelState()
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<LessonAddPasswordInputModel>()
                .Passing(model => model.LessonPassword == inputModel.LessonPassword && model.Id == lesson.Id));
        }

        [Fact]
        public void AddPassword_WithValidInputDataAndLessonWithPassword_ShouldReturnInfoMessageThatLessonAlreadyHasPassword()
        {
            Course course = CourseTestData.GetEntity();
            Lesson lesson = LessonTestData.GetEntity();
            lesson.LessonPassword = "already has password";
            var inputModel = new LessonAddPasswordInputModel
            {
                Id = lesson.Id,
                LessonPassword = "valid password"
            };

            MyController<LessonController>
            .Instance()
            .WithData(lesson)
            .Calling(c => c.AddPassword(inputModel))
            .ShouldHave()
            .TempData(tempData => tempData
                .ContainingEntry(entry => entry
                    .WithKey(GlobalConstants.ErrorKey)
                    .WithValue(ErrorMessages.LockedLesson)))
            .AndAlso()
            .ShouldReturn()
            .Redirect(result => result.To<Web.Controllers.CourseController>(c => c.Lessons(course.Id)));
        }

        [Fact]
        public void AddPassword_WithValidInputData_ShouldSetTheLessonPasswordAddInfoMessageInTempData()
        {
            Course course = CourseTestData.GetEntity();
            Lesson lesson = LessonTestData.GetEntity();
            var inputModel = new LessonAddPasswordInputModel
            {
                Id = lesson.Id,
                LessonPassword = "valid password"
            };

            MyController<LessonController>
            .Instance()
            .WithData(lesson)
            .Calling(c => c.AddPassword(inputModel))
            .ShouldHave()
            .Data(data => data.WithSet<Lesson>(set =>
            {
                string lessonPassword = set.Find(lesson.Id).LessonPassword;
                IPasswordHashService passwordHashService = new PasswordHashService();
                Assert.Equal(lessonPassword, passwordHashService.HashPassword(inputModel.LessonPassword));
            }))
            .AndAlso()
            .ShouldHave()
            .TempData(tempData => tempData
                .ContainingEntry(entry => entry
                    .WithKey(GlobalConstants.InfoKey)
                    .WithValue(string.Format(InfoMessages.AddLessonPasswordSuccessfully, lesson.Name))))
            .AndAlso()
            .ShouldReturn()
            .Redirect(result => result.To<Web.Controllers.CourseController>(c => c.Lessons(course.Id)));
        }

        [Fact]
        public void ChangePassword_ShouldReturnView() =>
            MyController<LessonController>
            .Instance()
            .Calling(c => c.ChangePassword())
            .ShouldReturn()
            .View();

        [Fact]
        public void ChangePassword_ShouldHaveAttribtesForPostRequestAndAntiForgeryToken() =>
            MyController<LessonController>
            .Instance()
            .Calling(c => c.ChangePassword(With.Default<LessonChangePasswordInputModel>()))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .ValidatingAntiForgeryToken());

        [Theory]
        [InlineData(TestConstnts.StringLessThan3Symbols, null)]
        [InlineData(TestConstnts.StringMoreThan50Symbols, "old password")]
        public void ChangePassword_WithInvalidInputData_ShouldHaveInvalidModelStateAndReturnViewWithTheSameViewModel(string newPassword, string oldPassword)
        {
            Lesson lesson = LessonTestData.GetEntity();
            var inputModel = new LessonChangePasswordInputModel
            {
                Id = lesson.Id,
                OldPassword = oldPassword,
                NewPassword = newPassword
            };

            MyController<LessonController>
            .Instance()
            .WithData(lesson)
            .Calling(c => c.ChangePassword(inputModel))
            .ShouldHave()
            .InvalidModelState()
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<LessonChangePasswordInputModel>()
                .Passing(model => model.OldPassword == inputModel.OldPassword && model.Id == lesson.Id && model.NewPassword == newPassword));
        }

        [Fact]
        public void ChangePassword_WithDifferentOldPassword_ShouldHaveInvalidModelState()
        {
            Lesson lesson = LessonTestData.GetEntity();
            lesson.LessonPassword = new PasswordHashService().HashPassword("old password");
            var inputModel = new LessonChangePasswordInputModel
            {
                Id = lesson.Id,
                NewPassword = "valid password",
                OldPassword = "wrong old password"
            };

            MyController<LessonController>
            .Instance()
            .WithData(lesson)
            .Calling(c => c.ChangePassword(inputModel))
            .ShouldHave()
            .ModelState(modelState => modelState
                .For<LessonChangePasswordInputModel>()
                .ContainingErrorFor(m => m.OldPassword)
                .Equals(ErrorMessages.DiffrentLessonPasswords))
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<LessonChangePasswordInputModel>()
                .Passing(model => model.OldPassword == inputModel.OldPassword &&
                                  model.Id == lesson.Id &&
                                  model.NewPassword == inputModel.NewPassword));
        }

        [Fact]
        public void ChangePassword_WithValidInputData_ShouldChangeThePasswordAddSetInfoMessageInTempData()
        {
            var hashSetvice = new PasswordHashService();
            Course course = CourseTestData.GetEntity();
            Lesson lesson = LessonTestData.GetEntity();
            string password = "secret password";
            lesson.LessonPassword = hashSetvice.HashPassword(password);
            var inputModel = new LessonChangePasswordInputModel
            {
                Id = lesson.Id,
                NewPassword = "valid password",
                OldPassword = password
            };

            MyController<LessonController>
            .Instance()
            .WithData(lesson)
            .Calling(c => c.ChangePassword(inputModel))
            .ShouldHave()
            .Data(data => data.WithSet<Lesson>(set =>
            {
                string lessonPassword = set.Find(lesson.Id).LessonPassword;
                Assert.Equal(lessonPassword, hashSetvice.HashPassword(inputModel.NewPassword));
            }))
            .AndAlso()
            .ShouldHave()
            .TempData(tempData => tempData
                .ContainingEntry(entry => entry
                    .WithKey(GlobalConstants.InfoKey)
                    .WithValue(string.Format(InfoMessages.ChangeLessonPasswordSuccessfully, lesson.Name))))
            .AndAlso()
            .ShouldReturn()
            .Redirect(result => result.To<Web.Controllers.CourseController>(c => c.Lessons(course.Id)));
        }

        [Fact]
        public void RemovePassword_ShouldReturnView() =>
           MyController<LessonController>
           .Instance()
           .Calling(c => c.RemovePassword())
           .ShouldReturn()
           .View();

        [Fact]
        public void RemovePassword_ShouldHaveAttribtesForPostRequestAndAntiForgeryToken() =>
            MyController<LessonController>
            .Instance()
            .Calling(c => c.RemovePassword(new LessonRemovePasswordInputModel()))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .ValidatingAntiForgeryToken());

        [Theory]
        [InlineData(null)]
        public void RemovePassword_WithInvalidInputData_ShouldHaveInvalidModelStateAndReturnViewWithTheSameViewModel(string oldPassword)
        {
            Lesson lesson = LessonTestData.GetEntity();
            var inputModel = new LessonRemovePasswordInputModel
            {
                Id = lesson.Id,
                OldPassword = oldPassword
            };

            MyController<LessonController>
            .Instance()
            .WithData(lesson)
            .Calling(c => c.RemovePassword(inputModel))
            .ShouldHave()
            .InvalidModelState()
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<LessonRemovePasswordInputModel>()
                .Passing(model => model.OldPassword == inputModel.OldPassword && model.Id == lesson.Id));
        }

        [Fact]
        public void RemovePassword_WithInvalidOldPassword_ShouldAddErrorToModelState()
        {
            Lesson lesson = LessonTestData.GetEntity();
            lesson.LessonPassword = new PasswordHashService().HashPassword("other old password");
            var inputModel = new LessonRemovePasswordInputModel
            {
                Id = lesson.Id,
                OldPassword = "invalid old password"
            };

            MyController<LessonController>
            .Instance()
            .WithData(lesson)
            .Calling(c => c.RemovePassword(inputModel))
            .ShouldHave()
            .ModelState(modelState => modelState
                .For<LessonRemovePasswordInputModel>()
                .ContainingErrorFor(m => m.OldPassword)
                .Equals(ErrorMessages.DiffrentLessonPasswords))
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<LessonRemovePasswordInputModel>()
                .Passing(model => model.Id == lesson.Id && model.OldPassword == inputModel.OldPassword));
        }

        [Fact]
        public void RemovePassword_WithValidInputData_ShouldRemoveTheLessonPasswordAddInfoMessageInTempData()
        {
            var hashService = new PasswordHashService();
            Course course = CourseTestData.GetEntity();
            Lesson lesson = LessonTestData.GetEntity();
            string oldPassword = "old password";
            lesson.LessonPassword = hashService.HashPassword(oldPassword);
            var inputModel = new LessonRemovePasswordInputModel
            {
                Id = lesson.Id,
                OldPassword = oldPassword
            };

            MyController<LessonController>
            .Instance()
            .WithData(lesson)
            .Calling(c => c.RemovePassword(inputModel))
            .ShouldHave()
            .Data(data => data.WithSet<Lesson>(set =>
            {
                string lessonPassword = set.Find(lesson.Id).LessonPassword;
                Assert.Null(lessonPassword);
            }))
            .AndAlso()
            .ShouldHave()
            .TempData(tempData => tempData
                .ContainingEntry(entry => entry
                    .WithKey(GlobalConstants.InfoKey)
                    .WithValue(string.Format(InfoMessages.LessonPasswordRemoved, lesson.Name))))
            .AndAlso()
            .ShouldReturn()
            .Redirect(result => result.To<Web.Controllers.CourseController>(c => c.Lessons(course.Id)));
        }
    }
}
