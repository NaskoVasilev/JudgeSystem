using System.Linq;

using JudgeSystem.Data.Models;
using JudgeSystem.Web.Controllers;
using JudgeSystem.Web.InputModels.Feedback;

using MyTested.AspNetCore.Mvc;
using Xunit;
using Shouldly;

namespace JudgeSystem.Web.Tests.Controllers
{
    public class FeedbackControllerTests
    {
        [Fact]
        public void FeedbackControllerActionsShouldBeAllowedOnlyForAuthorizedUser() =>
            MyController<FeedbackController>
            .Instance()
            .ShouldHave()
            .Attributes(attributes => attributes.RestrictingForAuthorizedRequests());

        [Fact]
        public void SendShouldReturnView() =>
            MyController<FeedbackController>
            .Instance()
            .Calling(c => c.Send())
            .ShouldReturn()
            .View();

        [Fact]
        public void SendShouldBeAllowedOnlyForPostRequest() =>
            MyController<FeedbackController>
            .Instance()
            .Calling(c => c.Send(With.Default<FeedbackCreateInputModel>()))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post));

        [Fact]
        public void SendShouldReturnViewWithTheSameViewModelWhenModelStateIsInvalid() =>
            MyController<FeedbackController>
            .Instance()
            .Calling(c => c.Send(With.Default<FeedbackCreateInputModel>()))
            .ShouldHave()
            .InvalidModelState()
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<FeedbackCreateInputModel>()
                .Passing(model => model.Content == null));

        [Theory]
        [InlineData("MyTested.AspNetCore.Mvc works fine")]
        public void SendShouldReturnRedirectAndShoudAddFeedbackWithValidFeedback(string content) =>
            MyController<FeedbackController>
            .Instance()
            .WithUser(user => user.WithUsername("nasko"))
            .Calling(c => c.Send(new FeedbackCreateInputModel { Content = content }))
            .ShouldHave()
            .Data(data => data
                .WithSet<Feedback>(set =>
                {
                    set.ShouldNotBeNull();
                    set.FirstOrDefault(f => f.Content == content).ShouldNotBeNull();
                }))
            .AndAlso()
            .ShouldReturn()
            .Redirect(result => result.To<HomeController>(c => c.Index()));
    }
}
