using JudgeSystem.Web.Controllers;

using MyTested.AspNetCore.Mvc;
using Xunit;

namespace JudgeSystem.Web.Tests.Controllers
{
    public class ProblemControllerTests
    {
        [Fact]
        public void ProblemControllerActionsShouldBeAllowedOnlyForAuthorizedUsers() =>
           MyController<ProblemController>
           .Instance()
           .ShouldHave()
           .Attributes(attributes => attributes.RestrictingForAuthorizedRequests());
    }
}
