using JudgeSystem.Common;
using JudgeSystem.Web.Controllers;

using MyTested.AspNetCore.Mvc;
using Xunit;

namespace JudgeSystem.Web.Tests.Controllers
{
    public class BaseControllerTests
    {
        private const string ErrorMessage = "Some error message.";
        private const string Action = "Index";
        private const string Controller = "HomeController";
        private const string Area = GlobalConstants.AdministrationArea;
        private const string InfoMessage = "Some info message";

        [Fact]
        public void ShowError_WithErrorMessageActionAndController_ShouldReturnRedirectResultAndAddErrorToTempData() =>
            MyController<BaseController>
            .Instance()
            .Calling(c => c.ShowError(ErrorMessage, Action, Controller))
            .ShouldHave()
            .TempData(tempData => tempData.ContainingEntry(GlobalConstants.ErrorKey, ErrorMessage))
            .AndAlso()
            .ShouldReturn()
            .RedirectToAction(Action, Controller);

        [Fact]
        public void ShowError_WithErrorMessageActionControllerAndArea_ShouldReturnRedirectResultAndAddErrorToTempData() =>
           MyController<BaseController>
           .Instance()
           .Calling(c => c.ShowError(ErrorMessage, Action, Controller, new { Area }))
           .ShouldHave()
           .TempData(tempData => tempData.ContainingEntry(GlobalConstants.ErrorKey, ErrorMessage))
           .AndAlso()
           .ShouldReturn()
           .RedirectToAction(Action, Controller, new { Area });

        [Fact]
        public void ShowError_WithErrorMessageActionControllerAndRouteValues_ShouldReturnRedirectResultAndAddErrorToTempData()
        {
            var routeValues = new { Area, LessonId = 5, ContestId = 45 };
            MyController<BaseController>
            .Instance()
            .Calling(c => c.ShowError(ErrorMessage, Action, Controller, routeValues))
            .ShouldHave()
            .TempData(tempData => tempData.ContainingEntry(GlobalConstants.ErrorKey, ErrorMessage))
            .AndAlso()
            .ShouldReturn()
            .RedirectToAction(Action, Controller, routeValues);
        }

        [Fact]
        public void ShowInfo_WithINfoMessageActionAndController_ShouldReturnRedirectResultAndAddInfoMessageToTempData() =>
           MyController<BaseController>
           .Instance()
           .Calling(c => c.ShowInfo(InfoMessage, Action, Controller))
           .ShouldHave()
           .TempData(tempData => tempData.ContainingEntry(GlobalConstants.InfoKey, InfoMessage))
           .AndAlso()
           .ShouldReturn()
           .RedirectToAction(Action, Controller);

        [Fact]
        public void ShowInfo_WithInfoMessageActionControllerAndRouteValues_ShouldReturnRedirectResultAndAddInfoToTempData()
        {
            var routeValues = new { Area, LessonId = 5, ContestId = 45 };
            MyController<BaseController>
            .Instance()
            .Calling(c => c.ShowInfo(InfoMessage, Action, Controller, routeValues))
            .ShouldHave()
            .TempData(tempData => tempData.ContainingEntry(GlobalConstants.InfoKey, InfoMessage))
            .AndAlso()
            .ShouldReturn()
            .RedirectToAction(Action, Controller, routeValues);
        }

        [Fact]
        public void ShowInfo_WithInfoMessageActionAndRouteValues_ShouldReturnRedirectResultAndAddInfoToTempData()
        {
            var routeValues = new { Area, LessonId = 5, ContestId = 45 };
            MyController<BaseController>
            .Instance()
            .Calling(c => c.ShowInfo(InfoMessage, Action, routeValues))
            .ShouldHave()
            .TempData(tempData => tempData.ContainingEntry(GlobalConstants.InfoKey, InfoMessage))
            .AndAlso()
            .ShouldReturn()
            .RedirectToAction(Action, routeValues);
        }
    }
}
