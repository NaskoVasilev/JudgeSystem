using System;
using System.Linq;

using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Web.Areas.Administration.Controllers;
using JudgeSystem.Web.Infrastructure.Attributes.Validation;
using JudgeSystem.Web.InputModels.Contest;
using JudgeSystem.Web.Tests.TestData;

using MyTested.AspNetCore.Mvc;
using Shouldly;
using Xunit;

namespace JudgeSystem.Web.Tests.Administration.Controllers
{
    public class ContestControllerTests
    {
        [Fact]
        public void Create_ShouldReturnView() =>
           MyController<ContestController>
           .Instance()
           .Calling(c => c.Create())
           .ShouldReturn()
           .View();

        [Fact]
        public void Create_ShouldHaveAttribtesForPostRequestAndAntiForgeryToken() =>
            MyController<ContestController>
            .Instance()
            .Calling(c => c.Create(With.Default<ContestCreateInputModel>()))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .ValidatingAntiForgeryToken());

        [Theory]
        [InlineData(TestConstnts.StringLessThan3Symbols)]
        [InlineData(TestConstnts.StringMoreThan50Symbols)]
        public void Create_WithInvalidModelState_ShouldReturnViewWithTheSameViewModel(string name) =>
            MyController<ContestController>
            .Instance()
            .Calling(c => c.Create(new ContestCreateInputModel { Name = name }))
            .ShouldHave()
            .InvalidModelState()
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<ContestCreateInputModel>()
                .Passing(model => model.Name == name));

        [Fact]
        public void Create_WithEndTimeAndStartTimeInThePast_ShouldHaveInvalidModelState()
        {
            var inputModel = new ContestCreateInputModel
            {
                EndTime = DateTime.Now.AddDays(-2),
                StartTime = DateTime.Now.AddDays(-5),
                Name = TestConstnts.StringLong10Symbols
            };

            MyController<ContestController>
            .Instance()
            .Calling(c => c.Create(inputModel))
            .ShouldHave()
            .ModelState(modelState =>
            {
                modelState.For<ContestCreateInputModel>()
                .ContainingErrorFor(m => m.EndTime)
                .ThatEquals(AfterDateTimeNowAttribute.DefaultMessage)
                .AndAlso()
                .ContainingErrorFor(m => m.StartTime)
                .ThatEquals(AfterDateTimeNowAttribute.DefaultMessage);
            });
        }

        [Fact]
        public void Create_WithEndTimeBeforeStartTime_ShouldHaveInvalidModelStateWithCorrectErrorMessage()
        {
            var inputModel = new ContestCreateInputModel
            {
                EndTime = DateTime.Now.AddDays(2),
                StartTime = DateTime.Now.AddDays(5),
                Name = TestConstnts.StringLong10Symbols
            };

            MyController<ContestController>
            .Instance()
            .Calling(c => c.Create(inputModel))
            .ShouldHave()
            .ModelState(modelState => modelState
                .For<ContestCreateInputModel>()
                .ContainingErrorFor(m => m.EndTime)
                .ThatEquals(ContestCreateInputModel.StartEndTimeErrorMessage));
        }

        [Fact]
        public void Create_WithValidFeedback_ShouldReturnRedirectAndShoudAddFeedback()
        {
            Lesson lesson = LessonTestData.GetEntity();
            var inputModel = new ContestCreateInputModel
            {
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(3),
                LessonId = lesson.Id,
                Name = "C# OOP"
            };

            MyController<ContestController>
            .Instance()
            .WithUser("nasko", GlobalConstants.AdministratorRoleName)
            .WithData(lesson)
            .Calling(c => c.Create(inputModel))
            .ShouldHave()
            .Data(data => data
                .WithSet<Contest>(set =>
                {
                    set.ShouldNotBeNull();
                    set.FirstOrDefault(f => f.Name == inputModel.Name).ShouldNotBeNull();
                }))
            .AndAlso()
            .ShouldReturn()
            .Redirect(result => result.To<Web.Controllers.HomeController>(c => c.Index()));
        }
    }
}
