using System.Collections.Generic;
using System;
using System.Linq;

using JudgeSystem.Common;
using JudgeSystem.Web.Areas.Administration.Controllers;
using JudgeSystem.Web.Tests.TestData;
using JudgeSystem.Data.Models;
using JudgeSystem.Web.Filters;
using JudgeSystem.Web.ViewModels.Feedback;

using MyTested.AspNetCore.Mvc;
using Shouldly;
using Xunit;

namespace JudgeSystem.Web.Tests.Administration.Controllers
{
    public class FeedbackControllerTests
    {
        [Theory]
        [InlineData(1, 6)]
        [InlineData(2, 2)]
        [InlineData(3, 0)]
        public void All_WithFeedbacksInTheDb_ShouldReturnCorrectFeedbacks(int page, int expectedCount)
        {
            IEnumerable<Feedback> testData = FeedbackTestData.GenerateFeedbacks();
            int numberOfPages = (int)Math.Ceiling((double)testData.Count() / GlobalConstants.FeedbacksPerPage);

            MyController<FeedbackController>
            .Instance()
            .WithData(testData)
            .Calling(c => c.All(page))
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<AllFeedbacksViewModel>()
                .Passing(model =>
                {
                    model.PaginationData.CurrentPage.ShouldBe(page);
                    model.PaginationData.NumberOfPages.ShouldBe(numberOfPages);
                    model.PaginationData.Url = "/Administration/Feedback/All?page={0}";
                    model.Feedbacks.Count().ShouldBe(expectedCount);

                    foreach (FeedbackAllViewModel feedback in model.Feedbacks)
                    {
                        feedback.Content.ShouldNotBeNull();
                        feedback.SenderEmail.ShouldBe(TestApplicationUser.Email);
                        feedback.SenderUsername.ShouldBe(TestApplicationUser.Username);
                    }
                }));
        }

        [Fact]
        public void Archive_ShouldHaveProperAttributes() =>
            MyController<FeedbackController>
            .Instance()
            .WithData(FeedbackTestData.GetEntity())
            .Calling(c => c.Archive(1))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .ContainingAttributeOfType<EndpointExceptionFilter>()
                .RestrictingForHttpMethod(HttpMethod.Post));

        [Fact]
        public void Archive_WithValidId_ShouldArchiveTheFeedbackAndReturnRedirectResult()
        {
            Feedback feedback = FeedbackTestData.GetEntity();

            MyController<FeedbackController>
           .Instance()
           .WithData(feedback)
           .Calling(c => c.Archive(feedback.Id))
           .ShouldHave()
           .Data(data => data.WithSet<Feedback>(set =>
           {
               Assert.False(set.Any(f => f.Id == feedback.Id));
           }))
           .AndAlso()
           .ShouldReturn()
           .Ok(result => result.WithModelOfType<string>()
                .Passing(model => model == string.Format(InfoMessages.SuccessfullyDeletedMessage, "feedback")));
        }
    }
}
