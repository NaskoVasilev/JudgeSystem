using JudgeSystem.Data.Models;
using JudgeSystem.Web.Controllers;
using JudgeSystem.Web.Dtos.Problem;
using JudgeSystem.Web.Filters;
using JudgeSystem.Web.Tests.TestData;

using MyTested.AspNetCore.Mvc;
using Xunit;

namespace JudgeSystem.Web.Tests.Controllers
{
    public class ProblemControllerTests
    {
        [Fact]
        public void ProblemControllerActions_ShouldBeAllowedOnlyForAuthorizedUsers() =>
           MyController<ProblemController>
           .Instance()
           .ShouldHave()
           .Attributes(attributes => attributes.RestrictingForAuthorizedRequests());

        [Fact]
        public void Get_WithValidProblemId_ShouldReturnOkResultAndHaveEndpointExceptionFilter()
        {
            Problem problem = ProblemTestData.GetEntity();

            MyController<ProblemController>
            .Instance()
            .WithData(problem)
            .Calling(c => c.Get(problem.Id))
            .ShouldHave()
                .ActionAttributes(attributes => attributes.ContainingAttributeOfType<EndpointExceptionFilter>())
            .AndAlso()
            .ShouldReturn()
            .Ok(result => result
                .WithModelOfType<ProblemConstraintsDto>()
                .Passing(model =>
                {
                    Assert.Equal(problem.AllowedMemoryInMegaBytes, model.AllowedMemoryInMegaBytes);
                    Assert.Equal(problem.AllowedTimeInMilliseconds, model.AllowedTimeInMilliseconds);
                }));
        }
    }
}
