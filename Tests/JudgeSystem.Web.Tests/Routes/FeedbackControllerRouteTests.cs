using JudgeSystem.Web.Controllers;
using JudgeSystem.Web.InputModels.Feedback;

using MyTested.AspNetCore.Mvc;
using Xunit;

namespace JudgeSystem.Web.Tests.Routes
{
    public class FeedbackControllerRouteTests
    {
        [Fact]
        public void SendWithHttpGetMethodShouldBeRoutedCorrectly() =>
            MyRouting
            .Configuration()
            .ShouldMap(request => request
                .WithLocation("/Feedback/Send")
                .WithMethod(HttpMethod.Get)
                .WithUser())
            .To<FeedbackController>(c => c.Send());

        [Fact]
        public void SendWithHttpPostMethodShouldBeRoutedCorrectly() =>
            MyRouting
            .Configuration()
            .ShouldMap(request => request   
                .WithLocation("/Feedback/Send")
                .WithMethod(HttpMethod.Post)
                .WithUser()
                .WithAntiForgeryToken())
            .To<FeedbackController>(c => c.Send(With.Any<FeedbackCreateInputModel>()));
    }
}
