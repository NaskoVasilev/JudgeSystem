using System.Collections.Generic;
using System;
using System.Linq;

using JudgeSystem.Common;
using JudgeSystem.Web.Areas.Administration.Controllers;
using JudgeSystem.Web.Tests.TestData;
using JudgeSystem.Data.Models;
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
        public void AllShouldReturnCorrectFeedbacks(int page, int expectedCount)
        {
            IEnumerable<Feedback> testData = FeedbackTestData.GetData();
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
                    model.Feedbacks.Count().ShouldBe(expectedCount);

                    foreach (FeedbackAllViewModel feedback in model.Feedbacks)
                    {
                        feedback.Content.ShouldNotBeNull();
                        feedback.SenderEmail.ShouldBe(TestApplicationUser.Email);
                        feedback.SenderUsername.ShouldBe(TestApplicationUser.Username);
                    }
                }));
        }

    }
}
