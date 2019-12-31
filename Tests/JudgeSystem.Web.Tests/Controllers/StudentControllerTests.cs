using System.Linq;

using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Web.Controllers;
using JudgeSystem.Web.InputModels.Student;
using JudgeSystem.Web.Tests.TestData;
using JudgeSystem.Web.ViewModels.Student;

using Microsoft.AspNetCore.Identity;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace JudgeSystem.Web.Tests.Controllers
{
    public class StudentControllerTests
    {
        [Fact]
        public void StudentControllerActions_ShouldBeAllowedOnlyForAuthorizedUsers() =>
           MyController<StudentController>
           .Instance()
           .ShouldHave()
                .Attributes(attributes => attributes.RestrictingForAuthorizedRequests());

        [Fact]
        public void ActivateStudentProfile_ShouldReturnView() =>
            MyController<StudentController>
            .Instance()
            .WithUser()
            .Calling(c => c.ActivateStudentProfile())
            .ShouldReturn()
            .View();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ActivateStudentProfile_WithInvalidModelState_ShouldReturnViewWithTheSameModel(string activationKey) =>
            MyController<StudentController>
            .Instance()
            .WithUser()
            .Calling(c => c.ActivateStudentProfile(new StudentActivateProfileInputModel { ActivationKey = activationKey }))
            .ShouldHave()
            .InvalidModelState()
            .AndAlso()
            .ShouldReturn()
            .View(result => result.WithModelOfType<StudentActivateProfileInputModel>());

        [Fact]
        public void ActivateStudentProfile_WithInvalidActiovationKey_ShouldReturnViewWithTheSameModelAndAddModelError()
        {
            Student student = StudentTestData.GetEntity();
            string activationKey = "wrong activation key";

            MyController<StudentController>
           .Instance()
           .WithUser()
           .Calling(c => c.ActivateStudentProfile(new StudentActivateProfileInputModel { ActivationKey = activationKey }))
           .ShouldHave()
           .ModelState(state => state
                .For<StudentActivateProfileInputModel>()
                .ContainingErrorFor(m => m.ActivationKey)
                .ThatEquals(ErrorMessages.InvalidActivationKey))
           .AndAlso()
           .ShouldReturn()
           .View(result => result.WithModelOfType<StudentActivateProfileInputModel>());
        }

        [Fact]
        public void ActivateStudentProfile_WithStudentWithActivatedStudentAccount_ShouldReturnViewWithTheSameModelAndAddModelError()
        {
            Student student = StudentTestData.GetEntity();
            student.IsActivated = true;

            MyController<StudentController>
           .Instance()
           .WithData(student)
           .WithUser()
           .Calling(c => c.ActivateStudentProfile(new StudentActivateProfileInputModel { ActivationKey = StudentTestData.StudentActivationKey }))
           .ShouldHave()
           .ModelState(state => state
                .ContainingError(string.Empty)
                .ThatEquals(ErrorMessages.ActivatedStudentProfile))
           .AndAlso()
           .ShouldReturn()
           .View(result => result.WithModelOfType<StudentActivateProfileInputModel>());
        }

        [Fact]
        public void ActivateStudentProfile_WithValidActivationKey_ShouldResirectAndAddStudentRoleToUser()
        {
            Student student = StudentTestData.GetEntity();
            ApplicationUser user = TestApplicationUser.GetDefaultUser();
            ApplicationRole role = RoleTestData.GetEntity(GlobalConstants.StudentRoleName);

            MyController<StudentController>
           .Instance()
           .WithData(user, student, role)
           .WithUser()
           .Calling(c => c.ActivateStudentProfile(new StudentActivateProfileInputModel { ActivationKey = StudentTestData.StudentActivationKey }))
           .ShouldHave()
           .Data(data => data
               .WithSet<Student>(set => Assert.NotNull(set.First(s => s.Id == student.Id && s.IsActivated)))
               .WithSet<ApplicationUser>(set =>
               {
                   ApplicationUser targetUser = set.First(u => u.Id == user.Id);
                   Assert.NotNull(targetUser);
                   Assert.Equal(student.Id, targetUser.StudentId);
               })
               .WithSet<IdentityUserRole<string>>(set => Assert.True(set.Any(x => x.RoleId == role.Id && x.UserId == user.Id))))
           .AndAlso()
           .ShouldReturn()
           .Redirect("/Identity/Account/Manage");
        }

        [Fact]
        public void Profile_ShouldBeAllowedOnlyForUsersInRoleStudent() => MyController<StudentController>
            .Instance()
            .WithData(TestApplicationUser.GetDefaultUser())
            .WithUser()
            .Calling(c => c.Profile())
            .ShouldHave()
            .ActionAttributes(attributes => attributes.RestrictingForAuthorizedRequests(GlobalConstants.StudentRoleName));

        [Fact]
        public void Profile_WithUserAuthenticatedUserInRoleStudent_ShouldReturnViewWithCorrectData()
        {
            ApplicationUser user = TestApplicationUser.GetDefaultUser();
            Student student = StudentTestData.GetEntity();
            user.Student = student;

            MyController<StudentController>
            .Instance()
            .WithData(student, user)
            .WithUser(user => user.InRole(GlobalConstants.StudentRoleName))
            .Calling(c => c.Profile())
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<StudentProfileViewModel>()
                .Passing(model =>
                {
                    Assert.Equal(student.Email, model.Email);
                    Assert.Equal(student.Id, model.Id);
                    Assert.Equal($"{student.SchoolClass.ClassNumber} {student.SchoolClass.ClassType}", model.SchoolClassName);
                }));
        }
    }
}
