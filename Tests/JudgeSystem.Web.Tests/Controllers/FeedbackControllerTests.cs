using System.Linq;

using JudgeSystem.Data.Models;
using JudgeSystem.Web.Controllers;
using JudgeSystem.Web.InputModels.Feedback;
using JudgeSystem.Common;

using MyTested.AspNetCore.Mvc;
using Xunit;
using Shouldly;

namespace JudgeSystem.Web.Tests.Controllers
{
    public class FeedbackControllerTests
    {
        [Fact]
        public void FeedbackControllerActions_ShouldBeAllowedOnlyForAuthorizedUser() =>
            MyController<FeedbackController>
            .Instance()
            .ShouldHave()
            .Attributes(attributes => attributes.RestrictingForAuthorizedRequests());

        [Fact]
        public void Send_ShouldReturnView() =>
            MyController<FeedbackController>
            .Instance()
            .Calling(c => c.Send())
            .ShouldReturn()
            .View();

        [Fact]
        public void Send_ShouldHaveAttribtesForPostRequestAndAntiForgeryToken() =>
            MyController<FeedbackController>
            .Instance()
            .Calling(c => c.Send(With.Default<FeedbackCreateInputModel>()))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .ValidatingAntiForgeryToken());

        [Theory]
        [InlineData(ModelConstants.FeedbackContentMaxLength + 1)]
        [InlineData(ModelConstants.FeedbackContentMinLength - 1)]
        public void Send_WithInvalidModelStateShouldReturnViewWithTheSameViewModel(int contentLength)
        {
            string content = new string('a', contentLength);

            MyController<FeedbackController>
           .Instance()
           .Calling(c => c.Send(new FeedbackCreateInputModel { Content = content }))
           .ShouldHave()
           .ModelState(modekState => modekState.For<FeedbackCreateInputModel>().ContainingErrorFor(m => m.Content))
           .AndAlso()
           .ShouldReturn()
           .View(result => result
               .WithModelOfType<FeedbackCreateInputModel>()
               .Passing(model => model.Content == content));
        }
           
        [Theory]
        [InlineData("Jydge System works fine")]
        public void Send_WithValidFeedback_ShouldReturnRedirectAndShoudAddFeedback(string content) =>
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
