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
        public void Details_WithNotNullableContestIdAndAuthorizedUser_ShouldAddErrorMessageToTheTempDataAndRedirectToHomePage() =>
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
            int contestId = 1;
            Lesson lesson = LessonTestData.GetEntity();
            Contest contest = ContestTestData.GetEntity();
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
            int lessonId = 1;
            int practiceId = 1;
            Lesson lesson = LessonTestData.GetEntity();

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
            .View(result => result.WithModelOfType<LessonViewModel>());
        }

        [Fact]
        public void Details_WithLockedLessonUserInRoleAdministrator_ShouldBeAccessible()
        {
            int lessonId = 1;
            int practiceId = 1;
            Lesson lesson = LessonTestData.GetEntity();
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
        public void Details_WithLockedLessonAndNoAdministratorAndDataInSession_ShouldBeAccessible()
        {
            int lessonId = 1;
            int practiceId = 1;
            Lesson lesson = LessonTestData.GetEntity();
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
        public void Details_WithLockedLessonAndNoAdministratorAndDataInSessionWithDifferentUsername_ShouldRedirectToEnterPasswordAction()
        {
            int lessonId = 1;
            int practiceId = 1;
            Lesson lesson = LessonTestData.GetEntity();
            lesson.LessonPassword = "some secret password";

            MyController<LessonController>
           .Instance()
           .WithData(lesson)
           .WithUser()
           .WithSession(session => session.WithEntry(lesson.Id.ToString(), Encoding.UTF8.GetBytes("wrong username")))
           .Calling(c => c.Details(lessonId, null, practiceId))
           .ShouldReturn()
           .RedirectToAction("EnterPassword", new { id = lesson.Id, practiceId });
        }

        [Fact]
        public void Details_WithLockedLessonAndNoAdministratorAndNoDataInTheSession_ShouldRedirectToEnterPassword()
        {
            int lessonId = 1;
            int contestId = 1;
            Lesson lesson = LessonTestData.GetEntity();
            lesson.LessonPassword = "some secret password";

            MyController<LessonController>
           .Instance()
           .WithData(lesson)
           .WithUser(user => user.InRole(GlobalConstants.StudentRoleName))
           .Calling(c => c.Details(lessonId, contestId, null))
           .ShouldReturn()
           .RedirectToAction("EnterPassword", new { id = lesson.Id, contestId });
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
        public void EneterPassword_WithLessonWithValidPasswordAndContestId_ShouldHaveValidAttributesAndRedirectToDetailsWithCintestIdAsParameter()
        {
            Lesson lesson = LessonTestData.GetEntity();
            string password = "testpass";
            lesson.LessonPassword = new PasswordHashService().HashPassword(password);
            Contest contest = ContestTestData.GetEntity();
            var model = new LessonPasswordInputModel() { LessonPassword = password, Id = lesson.Id, ContestId = contest.Id };

            MyController<LessonController>
           .Instance()
           .WithUser()
           .WithData(lesson, contest)
           .Calling(c => c.EnterPassword(model))
           .ShouldHave()
           .ActionAttributes(attributes => attributes.RestrictingForHttpMethod(HttpMethod.Post).ValidatingAntiForgeryToken())
           .AndAlso()
           .ShouldReturn()
           .RedirectToAction("Details", new { lesson.Id, contestId = model.ContestId });
        }

        [Fact]
        public void EneterPassword_WithLessonWithValidPasswordAndPracticeId_ShouldRedirectToDetailsWithPracticeIdAsParameterAndSetDataInTheSession()
        {
            Lesson lesson = LessonTestData.GetEntity();
            string password = "testpass";
            lesson.LessonPassword = new PasswordHashService().HashPassword(password);

            var model = new LessonPasswordInputModel() { LessonPassword = password, Id = lesson.Id, PracticeId = lesson.Practice.Id };

            MyController<LessonController>
           .Instance()
           .WithUser()
           .WithData(lesson)
           .Calling(c => c.EnterPassword(model))
           .ShouldHave()
           .Session(session => session.ContainingEntry(lesson.Id.ToString(), TestUser.Username))
           .AndAlso()
           .ShouldReturn()
           .RedirectToAction("Details", new { lesson.Id, practiceId = model.PracticeId });
        }

        [Fact]
        public void EneterPassword_WithInvalidLessonPassword_ShoudlReturnViewWithTheSameModelAndAddErrorToTheModelState()
        {
            Lesson lesson = LessonTestData.GetEntity();
            var model = new LessonPasswordInputModel() { LessonPassword = "incorrect", Id = lesson.Id, PracticeId = lesson.Practice.Id };

            MyController<LessonController>
           .Instance()
           .WithUser()
           .WithData(lesson)
           .Calling(c => c.EnterPassword(model))
           .ShouldHave()
           .ModelState(state => state.For<LessonPasswordInputModel>().ContainingErrorFor(x => x.LessonPassword))
           .AndAlso()
           .ShouldReturn()
           .View(result =>result.WithModelOfType<LessonPasswordInputModel>());
        }
    }
}
