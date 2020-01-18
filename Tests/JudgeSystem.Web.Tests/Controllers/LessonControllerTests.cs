using System.Linq;
using System.Text;

using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Services;
using JudgeSystem.Web.Controllers;
using JudgeSystem.Web.InputModels.Lesson;
using JudgeSystem.Web.Tests.TestData;
using JudgeSystem.Web.ViewModels.Lesson;

using MyTested.AspNetCore.Mvc;
using Shouldly;
using Xunit;

namespace JudgeSystem.Web.Tests.Controllers
{
    public class LessonControllerTests
    {
        [Fact]
        public void LessonControllerActionsShouldBeAllowedOnlyForAuthorizedUsers() =>
           MyController<LessonController>
           .Instance()
           .ShouldHave()
                .Attributes(attributes => attributes.RestrictingForAuthorizedRequests());

        [Fact]
        public void Details_WithNotNullableContestIdAndUserWhoIsNotInRoleStudent_ShouldAddErrorMessageToTheTempDataAndRedirectToHomePage() =>
            MyController<LessonController>
            .Instance()
            .WithUser()
            .Calling(c => c.Details(1, 1, null))
            .ShouldHave()
            .TempData(tempData => tempData.ContainingEntry(GlobalConstants.InfoKey, ErrorMessages.ContestsAccessibleOnlyForStudents))
            .AndAlso()
            .ShouldReturn()
            .Redirect("/");

        [Fact]
        public void Details_WithNotNullableContestIdAndUserInRoleStudent_ShouldAddUserToTheContestAndReturnViewWithProperData()
        {
            int lessonId = 1;
            Lesson lesson = LessonTestData.GetEntity();
            Contest contest = ContestTestData.GetEntity();
            int contestId = contest.Id;
            Resource resource = ResourceTestData.GetEntity();
            Problem problem = ProblemTestData.GetEntity();

            MyController<LessonController>
            .Instance()
            .WithData(lesson, resource, problem)
            .WithUser(user => user.InRole(GlobalConstants.StudentRoleName))
            .Calling(c => c.Details(lessonId, contestId, null))
            .ShouldHave()
            .Data(data => data
                 .WithSet<UserContest>(set =>
                 {
                     set.ShouldNotBeEmpty();
                     UserContest userContest = set.FirstOrDefault(x => x.UserId == TestUser.Identifier && x.ContestId == contestId);
                     userContest.ShouldNotBeNull();
                 }))
            .AndAlso()
            .ShouldReturn()
            .View(result => result
            .WithModelOfType<LessonViewModel>()
            .Passing(model =>
            {
                model.Name.ShouldBe(lesson.Name);
                model.ContestId.ShouldBe(contestId);
                model.Id.ShouldBe(lessonId);
                model.Resources.ShouldNotBeEmpty();
                model.Resources.First().Name.ShouldBe(resource.Name);
                model.Problems.ShouldNotBeEmpty();
                model.Problems.First().Name.ShouldBe(problem.Name);
            }));
        }

        [Fact]
        public void Details_WithNotNullablePracticeIdAndUser_ShouldAddUserToThePracticeAndReturnView()
        {
            Lesson lesson = LessonTestData.GetEntity();
            int lessonId = lesson.Id;
            int practiceId = lesson.Practice.Id;

            MyController<LessonController>
            .Instance()
            .WithData(lesson)
            .WithUser()
            .Calling(c => c.Details(lessonId, null, practiceId))
            .ShouldHave()
            .Data(data => data
                 .WithSet<UserPractice>(set =>
                 {
                     set.ShouldNotBeEmpty();
                     UserPractice userPractice = set.FirstOrDefault(x => x.UserId == TestUser.Identifier && x.PracticeId == lesson.Practice.Id);
                     userPractice.ShouldNotBeNull();
                 }))
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<LessonViewModel>()
                .Passing(model => model.Name == lesson.Name && 
                model.PracticeId == practiceId && 
                model.CourseId == lesson.Course.Id));
        }

        [Fact]
        public void Details_WithLockedLessonAndUserInRoleAdministrator_ShouldBeAccessible()
        {
            Lesson lesson = LessonTestData.GetEntity();
            int lessonId = lesson.Id;
            int practiceId = lesson.Practice.Id;
            lesson.LessonPassword = "some secret password";

            MyController<LessonController>
           .Instance()
           .WithData(lesson)
           .WithUser(user => user.InRole(GlobalConstants.AdministratorRoleName)) 
           .Calling(c => c.Details(lessonId, null, practiceId))
           .ShouldReturn()
           .View(result => result.WithModelOfType<LessonViewModel>());
        }

        [Fact]
        public void Details_WithLockedLessonNoAdministratorUserAndUserLessonDataInTheSession_ShouldBeAccessible()
        {
            Lesson lesson = LessonTestData.GetEntity();
            int lessonId = lesson.Id;
            int practiceId = lesson.Practice.Id;
            lesson.LessonPassword = "some secret password";

            MyController<LessonController>
           .Instance()
           .WithData(lesson)
           .WithUser()
           .WithSession(session => session.WithEntry(lesson.Id.ToString(), Encoding.UTF8.GetBytes(TestUser.Username)))
           .Calling(c => c.Details(lessonId, null, practiceId))
           .ShouldReturn()
           .View(result => result.WithModelOfType<LessonViewModel>());
        }

        [Fact]
        public void Details_WithLockedLessonNoAdministratorUserAndDataInSessionWithDifferentUsername_ShouldRedirectToEnterPasswordAction()
        {
            Lesson lesson = LessonTestData.GetEntity();
            int lessonId = lesson.Id;
            int practiceId = lesson.Practice.Id;
            lesson.LessonPassword = "some secret password";

            MyController<LessonController>
           .Instance()
           .WithData(lesson)
           .WithUser()
           .WithSession(session => session.WithEntry(lesson.Id.ToString(), Encoding.UTF8.GetBytes("wrong username")))
           .Calling(c => c.Details(lessonId, null, practiceId))
           .ShouldReturn()
           .RedirectToAction(nameof(LessonController.EnterPassword), new { id = lesson.Id, practiceId });
        }

        [Fact]
        public void Details_WithLockedLessonNoAdministratorUserAndEmptySession_ShouldRedirectToEnterPassword()
        {
            Lesson lesson = LessonTestData.GetEntity();
            int lessonId = lesson.Id;
            int contestId = 1;
            lesson.LessonPassword = "some secret password";

            MyController<LessonController>
           .Instance()
           .WithData(lesson)
           .WithUser(user => user.InRole(GlobalConstants.StudentRoleName))
           .Calling(c => c.Details(lessonId, contestId, null))
           .ShouldReturn()
           .RedirectToAction(nameof(LessonController.EnterPassword), new { id = lesson.Id, contestId });
        }

        [Fact]
        public void EneterPassword_WithNoParameters_ShouldReturnView() => 
            MyController<LessonController>
            .Instance()
            .WithUser()
            .Calling(c => c.EnterPassword())
            .ShouldReturn()
            .View();

        [Fact]
        public void EnterPassword_ShouldHaveValidateAntiforgeryTokenAndHttpPostAttributes() =>
            MyController<LessonController>
            .Instance()
            .WithData(LessonTestData.GetEntity())
            .Calling(c => c.EnterPassword(With.Default<LessonPasswordInputModel>()))
            .ShouldHave().ActionAttributes(attributes => attributes
               .RestrictingForHttpMethod(HttpMethod.Post)
               .ValidatingAntiForgeryToken());

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("123")]
        public void EnterPassword_WithInvalidInputData_ShouldHaveInvalidModelState(string password)
        {
            var model = new LessonPasswordInputModel()
            {
                LessonPassword = password,
            };

            MyController<LessonController>
            .Instance()
            .Calling(c => c.EnterPassword(model))
            .ShouldHave()
            .ModelState(modelState => modelState
                .For<LessonPasswordInputModel>()
                .ContainingErrorFor(m => m.LessonPassword))
            .AndAlso()
            .ShouldReturn()
            .View(result => result.WithModelOfType<LessonPasswordInputModel>());
        }

        [Fact]
        public void EneterPassword_WithValidPasswordAndContestId_ShouldRedirectToDetailsWithContestIdAsParameterAndSetUserLessonDataInTheSession()
        {
            Lesson lesson = LessonTestData.GetEntity();
            string password = "testpass";
            lesson.LessonPassword = new PasswordHashService().HashPassword(password);
            Contest contest = ContestTestData.GetEntity();
            var model = new LessonPasswordInputModel() 
            { 
                LessonPassword = password, 
                Id = lesson.Id, 
                ContestId = contest.Id 
            };

            MyController<LessonController>
           .Instance()
           .WithUser()
           .WithData(lesson, contest)
           .Calling(c => c.EnterPassword(model))
           .ShouldHave()
           .Session(session => session.ContainingEntry(lesson.Id.ToString(), TestUser.Username))
           .AndAlso()
           .ShouldReturn()
           .RedirectToAction(nameof(LessonController.Details), new { lesson.Id, contestId = model.ContestId });
        }

        [Fact]
        public void EneterPassword_WithValidPasswordAndPracticeId_ShouldRedirectToDetailsWithPracticeIdAsParameterAndSetUserLessonDataInTheSession()
        {
            Lesson lesson = LessonTestData.GetEntity();
            string password = "testpass";
            lesson.LessonPassword = new PasswordHashService().HashPassword(password);
            var model = new LessonPasswordInputModel() 
            { 
                LessonPassword = password, 
                Id = lesson.Id, 
                PracticeId = lesson.Practice.Id 
            };

            MyController<LessonController>
           .Instance()
           .WithUser()
           .WithData(lesson)
           .Calling(c => c.EnterPassword(model))
           .ShouldHave()
           .Session(session => session.ContainingEntry(lesson.Id.ToString(), TestUser.Username))
           .AndAlso()
           .ShouldReturn()
           .RedirectToAction(nameof(LessonController.Details), new { lesson.Id, practiceId = model.PracticeId });
        }

        [Fact]
        public void EneterPassword_WithInvalidLessonPassword_ShoudlReturnViewWithTheSameModelAndAddErrorInTheModelState()
        {
            Lesson lesson = LessonTestData.GetEntity();
            var model = new LessonPasswordInputModel()
            {
                LessonPassword = "incorrect", 
                Id = lesson.Id, 
                PracticeId = lesson.Practice.Id 
            };

            MyController<LessonController>
           .Instance()
           .WithUser()
           .WithData(lesson)
           .Calling(c => c.EnterPassword(model))
           .ShouldHave()
           .ModelState(state => state
                .For<LessonPasswordInputModel>()
                .ContainingErrorFor(x => x.LessonPassword))
           .AndAlso()
           .ShouldReturn()
           .View(result =>result
                .WithModelOfType<LessonPasswordInputModel>()
                .Passing(model => model.PracticeId == lesson.Practice.Id && model.Id == lesson.Id));
        }
    }
}
