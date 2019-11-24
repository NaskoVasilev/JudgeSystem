using JudgeSystem.Web.Controllers;

using MyTested.AspNetCore.Mvc;
using Xunit;

namespace JudgeSystem.Web.Tests.Controllers
{
    public class StudentControllerTests
    {
        [Fact]
        public void StudentControllerActionsShouldBeAllowedOnlyForAuthorizedUsers() =>
           MyController<StudentController>
           .Instance()
           .ShouldHave()
                .Attributes(attributes => attributes.RestrictingForAuthorizedRequests());
    }
}
