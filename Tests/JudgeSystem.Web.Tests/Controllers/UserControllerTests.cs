using JudgeSystem.Web.Controllers;

using MyTested.AspNetCore.Mvc;
using Xunit;

namespace JudgeSystem.Web.Tests.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public void UserControllerActionsShouldBeAllowedOnlyForAuthorizedUsers() =>
           MyController<UserController>
           .Instance()
           .ShouldHave()
                .Attributes(attributes => attributes.RestrictingForAuthorizedRequests());
    }
}
