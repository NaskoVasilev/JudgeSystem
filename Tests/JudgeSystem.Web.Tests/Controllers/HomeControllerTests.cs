using JudgeSystem.Web.Controllers;

using MyTested.AspNetCore.Mvc;
using Xunit;

namespace JudgeSystem.Web.Tests.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void PrivacyShouldReturnView() =>
            MyController<HomeController>
            .Instance()
            .Calling(c => c.Documentation())
            .ShouldReturn()
            .View();
    }
}
