using JudgeSystem.Web.Controllers;

using Microsoft.AspNetCore.Mvc;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace JudgeSystem.Web.Tests.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void Documentation_ShouldReturnView() =>
            MyController<HomeController>
            .Instance()
            .Calling(c => c.Documentation())
            .ShouldReturn()
            .View();

        [Fact]
        public void Index_ShouldReturnView() =>
            MyController<HomeController>
            .Instance()
            .Calling(c => c.Index())
            .ShouldReturn()
            .View();

        [Fact]
        public void Error_ShouldReturnViewAndShouldHaveResponceCacheAttribute() =>
            MyController<HomeController>
            .Instance()
            .Calling(c => c.Error())
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .CachingResponse(responseCache => responseCache
                    .WithDuration(0)
                    .WithLocation(ResponseCacheLocation.None)
                    .WithNoStore(true)))
            .AndAlso()
            .ShouldReturn()
            .View();
    }
}
