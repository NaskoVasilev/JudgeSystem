using JudgeSystem.Web.Controllers;

using MyTested.AspNetCore.Mvc;
using Xunit;

namespace JudgeSystem.Web.Tests.Controllers
{
    public class SubmissionControllerTests
    {
        [Fact]
        public void SubmissionControllerActionsShouldBeAllowedOnlyForAuthorizedUsers() =>
           MyController<SubmissionController>
           .Instance()
           .ShouldHave()
                .Attributes(attributes => attributes.RestrictingForAuthorizedRequests());
    }
}
