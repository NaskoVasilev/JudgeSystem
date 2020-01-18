using System.Linq;

using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Web.Areas.Administration.Controllers;
using JudgeSystem.Web.Controllers;
using JudgeSystem.Web.InputModels.Settings;
using JudgeSystem.Web.Tests.TestData;

using Microsoft.AspNetCore.Identity;

using MyTested.AspNetCore.Mvc;
using Xunit;

namespace JudgeSystem.Web.Tests.Administration.Controllers
{
    public class SettingsControllerTests
    {
        [Fact]
        public void AddAdministrator_ShouldBeAllowedOnlyForUsersInRoleOwnerAndReturnView() =>
           MyController<SettingsController>
           .Instance()
           .Calling(c => c.AddAdministrator())
           .ShouldHave()
           .ActionAttributes(attributes => attributes
                .RestrictingForAuthorizedRequests(GlobalConstants.OwnerRoleName))
           .AndAlso()
           .ShouldReturn()
           .View();

        [Fact]
        public void AddAdministrator_ShouldBeAllowedOnlyForUsersInRoleOwnerAndHaveHttpPostAndValidateAntiForgeryTokenAttributes() =>
           MyController<SettingsController>
           .Instance()
           .Calling(c => c.AddAdministrator(With.Default<UserIdentityConfirmationInputModel>()))
           .ShouldHave()
           .ActionAttributes(attributes => attributes
                .RestrictingForAuthorizedRequests(GlobalConstants.OwnerRoleName)
                .ValidatingAntiForgeryToken()
                .RestrictingForHttpMethod(HttpMethod.Post))
           .AndAlso()
           .ShouldReturn()
           .View();

        [Fact]
        public void AddAdministrator_WithInvalidInputData_ShouldHaveInvalidModelStateAndReturnViewWithTheSameViewModel()
        {
            var inputModel = new UserIdentityConfirmationInputModel
            {
                ConfirmPassword = null,
                Name = null,
                Surname = null,
                Username = null
            };

            MyController<SettingsController>
            .Instance()
            .Calling(c => c.AddAdministrator(inputModel))
            .ShouldHave()
            .ModelState(modelState => modelState
                .For<UserIdentityConfirmationInputModel>()
                .ContainingErrorFor(m => m.ConfirmPassword)
                .ContainingErrorFor(m => m.Name)
                .ContainingErrorFor(m => m.Surname)
                .ContainingErrorFor(m => m.Username))
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<UserIdentityConfirmationInputModel>());
        }

        [Fact]
        public void AddAdministrator_WithValidModelStateAndWithInvalidConfirmedPassword_ShouldSetErrorInTheModelState()
        {
            var inputModel = new UserIdentityConfirmationInputModel
            {
                ConfirmPassword = "wrong",
                Name = "test name",
                Surname = "test surname",
                Username = "test username"
            };

            MyController<SettingsController>
            .Instance()
            .WithUser(TestApplicationUser.Username, new string[] { GlobalConstants.AdministratorRoleName })
            .Calling(c => c.AddAdministrator(inputModel))
            .ShouldHave()
            .ModelState(modelState => modelState
                .For<UserIdentityConfirmationInputModel>()
                .ContainingErrorFor(m => m.ConfirmPassword)
                .Equals(ErrorMessages.InvalidPassword))
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<UserIdentityConfirmationInputModel>());
        }

        [Theory]
        [InlineData("invalid username", TestApplicationUser.Surname, TestApplicationUser.Name)]
        [InlineData(TestApplicationUser.Username, "invalid username", TestApplicationUser.Name)]
        [InlineData(TestApplicationUser.Username, TestApplicationUser.Surname, "invalid name")]
        [InlineData("invalid username", "invalid surname", "invalid name")]
        public void AddAdministrator_WithValidConfirmedPasswordAndInvalidUserData_ShouldSetErrorInTheModelState(string username, string name, string surname)
        {
            ApplicationUser administrator = TestApplicationUser.GetDefaultUser();
            administrator.UserName = "admin";
            administrator.Id = "admin_id";
            ApplicationUser user = TestApplicationUser.GetDefaultUser();
            var inputModel = new UserIdentityConfirmationInputModel
            {
                ConfirmPassword = TestApplicationUser.Password,
                Name = name,
                Surname = surname,
                Username = username
            };

            MyController<SettingsController>
            .Instance()
            .WithData(TestApplicationUser.GetDefaultUser(), administrator)
            .WithUser(administrator.UserName, new string[] { GlobalConstants.AdministratorRoleName })
            .Calling(c => c.AddAdministrator(inputModel))
            .ShouldHave()
            .ModelState(modelState => modelState
                .ContainingError(string.Empty)
                .Equals(ErrorMessages.UserNotFound))
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<UserIdentityConfirmationInputModel>());
        }

        [Fact]
        public void AddAdministrator_WithValidData_ShoudAddRoleAdminToPassedUserAndShowInfoMessage()
        {
            ApplicationUser administrator = TestApplicationUser.GetDefaultUser();
            administrator.UserName = "admin";
            administrator.Id = "admin_id";
            ApplicationUser user = TestApplicationUser.GetDefaultUser();
            var inputModel = new UserIdentityConfirmationInputModel
            {
                ConfirmPassword = TestApplicationUser.Password,
                Name = TestApplicationUser.Name,
                Surname = TestApplicationUser.Surname,
                Username = TestApplicationUser.Username
            };

            var role = new ApplicationRole
            {
                Id = "role_id",
                Name = GlobalConstants.AdministratorRoleName,
                NormalizedName = GlobalConstants.AdministratorRoleName.ToUpper()
            };

            MyController<SettingsController>
            .Instance()
            .WithData(user, administrator, role)
            .WithUser(administrator.UserName, new string[] { GlobalConstants.AdministratorRoleName })
            .Calling(c => c.AddAdministrator(inputModel))
            .ShouldHave()
            .ValidModelState()
            .AndAlso()
            .ShouldHave()
            .Data(data => data.WithSet<IdentityUserRole<string>>(set =>
            {
                bool userRoleExists = set.Any(i => i.RoleId == role.Id && i.UserId == TestApplicationUser.Id);
                Assert.True(userRoleExists);
            }))
            .AndAlso()
            .ShouldHave()
            .TempData(tempData => tempData
                .ContainingEntryWithKey(GlobalConstants.InfoKey)
                .Equals(string.Format(InfoMessages.AddAdministrator, inputModel.Username)))
            .AndAlso()
            .ShouldReturn()
            .Redirect(result => result.To<HomeController>(c => c.Index()));
        }

        [Fact]
        public void RemoveAdministrator_ShouldBeAllowedOnlyForUsersInRoleOwnerAndReturnView() =>
           MyController<SettingsController>
           .Instance()
           .Calling(c => c.RemoveAdministrator())
           .ShouldHave()
           .ActionAttributes(attributes => attributes
                .RestrictingForAuthorizedRequests(GlobalConstants.OwnerRoleName))
           .AndAlso()
           .ShouldReturn()
           .View();

        [Fact]
        public void RemoveAdministrator_ShouldBeAllowedOnlyForUsersInRoleOwnerAndHaveHttpPostAndValidateAntiForgeryTokenAttributes() =>
          MyController<SettingsController>
          .Instance()
          .Calling(c => c.RemoveAdministrator(With.Default<UserIdentityConfirmationInputModel>()))
          .ShouldHave()
          .ActionAttributes(attributes => attributes
               .RestrictingForAuthorizedRequests(GlobalConstants.OwnerRoleName)
               .ValidatingAntiForgeryToken()
               .RestrictingForHttpMethod(HttpMethod.Post))
          .AndAlso()
          .ShouldReturn()
          .View();

        [Fact]
        public void RemoveAdministrator_WithInvalidInputData_ShouldHaveInvalidModelStateAndReturnViewWithTheSameViewModel()
        {
            var inputModel = new UserIdentityConfirmationInputModel
            {
                ConfirmPassword = null,
                Name = null,
                Surname = null,
                Username = null
            };

            MyController<SettingsController>
            .Instance()
            .Calling(c => c.RemoveAdministrator(inputModel))
            .ShouldHave()
            .ModelState(modelState => modelState
                .For<UserIdentityConfirmationInputModel>()
                .ContainingErrorFor(m => m.ConfirmPassword)
                .ContainingErrorFor(m => m.Name)
                .ContainingErrorFor(m => m.Surname)
                .ContainingErrorFor(m => m.Username))
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<UserIdentityConfirmationInputModel>());
        }

        [Fact]
        public void RemoveAdministrator_WithValidModelStateAndWithInvalidConfirmedPassword_ShouldSetErrorInTheModelState()
        {
            var inputModel = new UserIdentityConfirmationInputModel
            {
                ConfirmPassword = "wrong",
                Name = "test name",
                Surname = "test surname",
                Username = "test username"
            };

            MyController<SettingsController>
            .Instance()
            .WithUser(TestApplicationUser.Username, new string[] { GlobalConstants.AdministratorRoleName })
            .Calling(c => c.RemoveAdministrator(inputModel))
            .ShouldHave()
            .ModelState(modelState => modelState
                .For<UserIdentityConfirmationInputModel>()
                .ContainingErrorFor(m => m.ConfirmPassword)
                .Equals(ErrorMessages.InvalidPassword))
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<UserIdentityConfirmationInputModel>());
        }

        [Theory]
        [InlineData("invalid username", TestApplicationUser.Surname, TestApplicationUser.Name)]
        [InlineData(TestApplicationUser.Username, "invalid username", TestApplicationUser.Name)]
        [InlineData(TestApplicationUser.Username, TestApplicationUser.Surname, "invalid name")]
        [InlineData("invalid username", "invalid surname", "invalid name")]
        public void RemoveAdministrator_WithValidConfirmedPasswordAndInvalidUserData_ShouldSetErrorInTheModelState(string username, string name, string surname)
        {
            ApplicationUser administrator = TestApplicationUser.GetDefaultUser();
            administrator.UserName = "admin";
            administrator.Id = "admin_id";
            ApplicationUser user = TestApplicationUser.GetDefaultUser();
            var inputModel = new UserIdentityConfirmationInputModel
            {
                ConfirmPassword = TestApplicationUser.Password,
                Name = name,
                Surname = surname,
                Username = username
            };

            MyController<SettingsController>
            .Instance()
            .WithData(user, administrator)
            .WithUser(administrator.UserName, new string[] { GlobalConstants.AdministratorRoleName })
            .Calling(c => c.RemoveAdministrator(inputModel))
            .ShouldHave()
            .ModelState(modelState => modelState
                .ContainingError(string.Empty)
                .Equals(ErrorMessages.UserNotFound))
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<UserIdentityConfirmationInputModel>());
        }

        [Fact]
        public void RemoveAdministrator_WithValidData_ShoudRemoveRoleAdminFromPassedUserAndShowInfoMessage()
        {
            ApplicationUser administrator = TestApplicationUser.GetDefaultUser();
            administrator.UserName = "admin";
            administrator.Id = "admin_id";
            ApplicationUser user = TestApplicationUser.GetDefaultUser();
            var inputModel = new UserIdentityConfirmationInputModel
            {
                ConfirmPassword = TestApplicationUser.Password,
                Name = TestApplicationUser.Name,
                Surname = TestApplicationUser.Surname,
                Username = TestApplicationUser.Username
            };

            var role = new ApplicationRole
            {
                Id = "role_id",
                Name = GlobalConstants.AdministratorRoleName,
                NormalizedName = GlobalConstants.AdministratorRoleName.ToUpper()
            };

            var userRole = new IdentityUserRole<string>() { RoleId = role.Id, UserId = user.Id };

            MyController<SettingsController>
            .Instance()
            .WithData(TestApplicationUser.GetDefaultUser(), administrator, role, userRole)
            .WithUser(administrator.UserName, new string[] { GlobalConstants.AdministratorRoleName })
            .Calling(c => c.RemoveAdministrator(inputModel))
            .ShouldHave()
            .ValidModelState()
            .AndAlso()
            .ShouldHave()
            .Data(data => data.WithSet<IdentityUserRole<string>>(set =>
            {
                bool userRoleExists = set.Any(i => i.RoleId == role.Id && i.UserId == TestApplicationUser.Id);
                Assert.False(userRoleExists);
            }))
            .AndAlso()
            .ShouldHave()
            .TempData(tempData => tempData
                .ContainingEntryWithKey(GlobalConstants.InfoKey)
                .Equals(string.Format(InfoMessages.RemoveAdministrator, inputModel.Username)))
            .AndAlso()
            .ShouldReturn()
            .Redirect(result => result.To<HomeController>(c => c.Index()));
        }
    }
}
