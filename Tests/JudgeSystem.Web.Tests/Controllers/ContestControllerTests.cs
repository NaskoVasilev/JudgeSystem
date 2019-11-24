using JudgeSystem.Web.Controllers;

using MyTested.AspNetCore.Mvc;
using Xunit;

namespace JudgeSystem.Web.Tests.Controllers
{
    public class ContestControllerTests
    {
        [Fact]
        public void ContestControllerActionsShouldBeAllowedOnlyForAuthorizedUsers() =>
           MyController<ContestController>
           .Instance()
           .ShouldHave()
           .Attributes(attributes => attributes.RestrictingForAuthorizedRequests());
    }
}
