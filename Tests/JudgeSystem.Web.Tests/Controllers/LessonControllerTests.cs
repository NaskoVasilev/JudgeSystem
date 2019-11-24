using JudgeSystem.Web.Controllers;

using MyTested.AspNetCore.Mvc;
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
    }
}
