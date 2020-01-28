using System;
using System.Linq;
using System.Collections.Generic;

using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Web.Controllers;
using JudgeSystem.Web.Filters;
using JudgeSystem.Web.Tests.TestData;
using JudgeSystem.Web.ViewModels.Practice;

using MyTested.AspNetCore.Mvc;
using Xunit;

namespace JudgeSystem.Web.Tests.Controllers
{
    public class PracticeControllerTests
    {
        [Fact]
        public void Results_WithValidPracticeId_ShouldReturnViewWithPracticeResultsAndPaginationData()
        {
            Practice practice = PracticeTestData.GetPracticeWithUserPractices();
            ApplicationUser firstUser = practice.UserPractices.First().User;

            MyController<PracticeController>
            .Instance()
            .WithData(practice)
            .Calling(c => c.Results(practice.Id, 1))
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<PracticeAllResultsViewModel>()
                .Passing(model =>
                {
                    Assert.Equal(practice.Lesson.Problems.Count, model.Problems.Count);
                    Assert.Single(model.PracticeResults);

                    PracticeResultViewModel firstUserResults = model.PracticeResults[0];
                    Assert.Equal(50, firstUserResults.PointsByProblem[1]);
                    Assert.Equal(100, firstUserResults.PointsByProblem[3]);
                    Assert.Equal(150, firstUserResults.Total);
                    Assert.Equal($"{firstUser.Name} {firstUser.Surname}", firstUserResults.FullName);
                    Assert.Equal(firstUser.UserName, firstUserResults.Username);
                    Assert.Equal(firstUser.Id, firstUserResults.UserId);

                    Assert.Equal(1, model.PaginationData.CurrentPage);
                    Assert.Equal(1, model.PaginationData.NumberOfPages);
                    Assert.Equal($"/Practice/{nameof(PracticeController.Results)}/{practice.Id}?page={{0}}", model.PaginationData.Url);
                }));
        }

        [Fact]
        public void ResultsPagesCount_WithValidId_ShouldReturnCorrectNumberOfPagesAndHaveEndpointExceptionFilter()
        {
            int entitesCount = 40;
            IEnumerable<UserPractice> userPracties = UserPracticeTestData.GenerateEntities(entitesCount);
            int expectedPages = (int)Math.Ceiling((double)entitesCount / PracticeController.ResultsPerPage);

            MyController<PracticeController>
            .Instance()
            .WithData(userPracties)
            .Calling(c => c.ResultsPagesCount(userPracties.First().PracticeId))
            .ShouldHave()
            .ActionAttributes(attributes => attributes.ContainingAttributeOfType<EndpointExceptionFilter>())
            .AndAlso()
            .ShouldReturn()
            .Result(expectedPages);
        }
    }
}
