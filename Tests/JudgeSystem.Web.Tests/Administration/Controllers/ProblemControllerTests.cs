using System.Linq;

using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Web.Areas.Administration.Controllers;
using JudgeSystem.Web.InputModels.Problem;
using JudgeSystem.Web.Tests.TestData;
using JudgeSystem.Web.ViewModels.Problem;

using MyTested.AspNetCore.Mvc;
using Xunit;

namespace JudgeSystem.Web.Tests.Administration.Controllers
{
    public class ProblemControllerTests
    {
        [Fact]
        public void Create_ShouldReturnViewWithModelWithDefaultValues() =>
          MyController<ProblemController>
          .Instance()
          .Calling(c => c.Create())
          .ShouldReturn()
          .View(result => result
              .WithModelOfType<ProblemInputModel>()
              .Passing(model =>
              {
                  Assert.Equal(GlobalConstants.DefaultAllowedMemoryInMegaBytes, model.AllowedMemoryInMegaBytes);
                  Assert.Equal(GlobalConstants.DefaultAllowedTimeInMilliseconds, model.AllowedTimeInMilliseconds);
                  Assert.Equal(GlobalConstants.DefaultMaxPoints, model.MaxPoints);
              }));

        [Fact]
        public void Create_ShouldHaveAttribtesForPostRequestAndAntiForgeryToken() =>
            MyController<ProblemController>
            .Instance()
            .Calling(c => c.Create(With.Default<ProblemInputModel>()))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .ValidatingAntiForgeryToken());

        [Theory]
        [InlineData(TestConstnts.StringLessThan3Symbols, ModelConstants.ProblemMinPoints - 1, GlobalConstants.MaxAllowedTimeInMilliseconds + 1, GlobalConstants.MinAllowedMemoryInMegaBytes - 1)]
        [InlineData(TestConstnts.StringMoreThan50Symbols, ModelConstants.ProblemMaxPoints + 1, GlobalConstants.MinAllowedTimeInMilliseconds - 1, GlobalConstants.MaxAllowedMemoryInMegaBytes + 1)]
        public void Create_WithInvalidInputData_ShouldHaveInvalidModelStateAndReturnViewWithTheSameViewModel(
            string name,
            int maxPoints,
            int allowedTimeInMilliseconds,
            int allowedMemoryInMegaBytes)
        {
            var inputModel = new ProblemInputModel
            {
                Name = name,
                AllowedMemoryInMegaBytes = allowedMemoryInMegaBytes,
                AllowedTimeInMilliseconds = allowedTimeInMilliseconds,
                MaxPoints = maxPoints,
            };

            MyController<ProblemController>
            .Instance()
            .Calling(c => c.Create(inputModel))
            .ShouldHave()
            .ModelState(modelState => modelState
                .For<ProblemInputModel>()
                .ContainingErrorFor(m => m.Name)
                .ContainingErrorFor(m => m.AllowedMemoryInMegaBytes)
                .ContainingErrorFor(m => m.AllowedTimeInMilliseconds)
                .ContainingErrorFor(m => m.MaxPoints))
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<ProblemInputModel>()
                .Passing(model => model.Name == name &&
                         model.MaxPoints == inputModel.MaxPoints &&
                         model.AllowedTimeInMilliseconds == inputModel.AllowedTimeInMilliseconds &&
                         model.AllowedMemoryInMegaBytes == inputModel.AllowedMemoryInMegaBytes));
        }

        [Fact]
        public void Create_WithValidInputData_ShouldReturnRedirectResultAndShoudAddTheProblemInTheDb()
        {
            int createdProblemId = 0;
            var inputModel = new ProblemInputModel
            {
                Name = "Reflection",
                LessonId = 1,
                SubmissionType = SubmissionType.PlainCode
            };

            MyController<ProblemController>
            .Instance()
            .Calling(c => c.Create(inputModel))
            .ShouldHave()
            .ValidModelState()
            .AndAlso()
            .ShouldHave()
            .Data(data => data
                .WithSet<Problem>(set =>
                {
                    Problem createdProblem = set.FirstOrDefault(p => p.Name == inputModel.Name);
                    createdProblemId = createdProblem.Id;
                    Assert.NotNull(createdProblem);
                    Assert.Equal(inputModel.Name, createdProblem.Name);
                    Assert.Equal(GlobalConstants.DefaultAllowedMemoryInMegaBytes, createdProblem.AllowedMemoryInMegaBytes);
                    Assert.Equal(GlobalConstants.DefaultAllowedTimeInMilliseconds, createdProblem.AllowedTimeInMilliseconds);
                    Assert.Equal(GlobalConstants.DefaultMaxPoints, createdProblem.MaxPoints);
                    Assert.Equal(inputModel.SubmissionType, createdProblem.SubmissionType);
                    Assert.Equal(inputModel.LessonId, createdProblem.LessonId);
                }))
            .AndAlso()
            .ShouldReturn()
            .Redirect(result => result.To<ProblemController>(c => c.AddTest(createdProblemId)));
        }

        [Fact]
        public void Edit_WithValidId_ShoudReturnViewWithCorrectModel()
        {
            Problem problem = ProblemTestData.GetEntity();

            MyController<ProblemController>
            .Instance()
            .WithData(problem)
            .Calling(c => c.Edit(problem.Id))
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<ProblemEditInputModel>()
                .Passing(model =>
                {
                    Assert.Equal(problem.Id, model.Id);
                    Assert.Equal(problem.Name, model.Name);
                    Assert.Equal(problem.MaxPoints, model.MaxPoints);
                    Assert.Equal(problem.AllowedTimeInMilliseconds, model.AllowedTimeInMilliseconds);
                    Assert.Equal(problem.AllowedMemoryInMegaBytes, model.AllowedMemoryInMegaBytes);
                    Assert.Equal(problem.SubmissionType, model.SubmissionType);
                }));
        }

        [Fact]
        public void Edit_ShouldHaveAttribtesForPostRequestAndAntiForgeryToken() =>
            MyController<ProblemController>
            .Instance()
            .Calling(c => c.Edit(With.Default<ProblemEditInputModel>()))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .ValidatingAntiForgeryToken());

        [Theory]
        [InlineData(TestConstnts.StringLessThan3Symbols, ModelConstants.ProblemMinPoints - 1, GlobalConstants.MaxAllowedTimeInMilliseconds + 1, GlobalConstants.MinAllowedMemoryInMegaBytes - 1)]
        [InlineData(TestConstnts.StringMoreThan50Symbols, ModelConstants.ProblemMaxPoints + 1, GlobalConstants.MinAllowedTimeInMilliseconds - 1, GlobalConstants.MaxAllowedMemoryInMegaBytes + 1)]
        public void Edit_WithInvalidInputData_ShouldHaveInvalidModelStateAndReturnViewWithTheSameViewModel(
           string name,
           int maxPoints,
           int allowedTimeInMilliseconds,
           int allowedMemoryInMegaBytes)
        {
            var inputModel = new ProblemEditInputModel
            {
                Name = name,
                AllowedMemoryInMegaBytes = allowedMemoryInMegaBytes,
                AllowedTimeInMilliseconds = allowedTimeInMilliseconds,
                MaxPoints = maxPoints,
            };

            MyController<ProblemController>
            .Instance()
            .Calling(c => c.Edit(inputModel))
            .ShouldHave()
            .ModelState(modelState => modelState
                .For<ProblemEditInputModel>()
                .ContainingErrorFor(m => m.Name)
                .ContainingErrorFor(m => m.AllowedMemoryInMegaBytes)
                .ContainingErrorFor(m => m.AllowedTimeInMilliseconds)
                .ContainingErrorFor(m => m.MaxPoints))
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<ProblemEditInputModel>()
                .Passing(model => model.Name == name &&
                         model.MaxPoints == inputModel.MaxPoints &&
                         model.AllowedTimeInMilliseconds == inputModel.AllowedTimeInMilliseconds &&
                         model.AllowedMemoryInMegaBytes == inputModel.AllowedMemoryInMegaBytes));
        }

        [Fact]
        public void Edit_WithValidInputData_ShouldReturnRedirectResultAndShouldUpdateTheProblem()
        {
            Problem problem = ProblemTestData.GetEntity();
            var inputModel = new ProblemEditInputModel
            {
                Id = problem.Id,
                Name = "C# OOP edited",
                SubmissionType = SubmissionType.PlainCode,
                IsExtraTask = true,
                MaxPoints = 50,
                AllowedMemoryInMegaBytes = 10,
                AllowedTimeInMilliseconds = 150
            };
            MyController<ProblemController>
            .Instance()
            .WithData(problem)
            .Calling(c => c.Edit(inputModel))
            .ShouldHave()
            .ValidModelState()
            .AndAlso()
            .ShouldHave()
            .Data(data => data
                .WithSet<Problem>(set =>
                {
                    Problem editedProblem = set.FirstOrDefault(c => c.Id == inputModel.Id);
                    Assert.NotNull(editedProblem);
                    Assert.Equal(inputModel.Name, editedProblem.Name);
                    Assert.Equal(inputModel.SubmissionType, editedProblem.SubmissionType);
                    Assert.Equal(inputModel.AllowedTimeInMilliseconds, editedProblem.AllowedTimeInMilliseconds);
                    Assert.Equal(inputModel.AllowedMemoryInMegaBytes, editedProblem.AllowedMemoryInMegaBytes);
                    Assert.Equal(inputModel.MaxPoints, editedProblem.MaxPoints);
                    Assert.Equal(inputModel.IsExtraTask, editedProblem.IsExtraTask);
                }))
            .AndAlso()
            .ShouldReturn()
            .Redirect(result => result.To<ProblemController>(c => c.All(problem.Lesson.Id)));
        }

        [Fact]
        public void Delete_WithValidId_ShoudReturnViewWithCorrectModel()
        {
            Problem problem = ProblemTestData.GetEntity();

            MyController<ProblemController>
            .Instance()
            .WithData(problem)
            .Calling(c => c.Delete(problem.Id))
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<ProblemViewModel>()
                .Passing(model =>
                {
                    Assert.Equal(problem.Id, model.Id);
                    Assert.Equal(problem.Name, model.Name);
                }));
        }

        [Fact]
        public void DeletePost_ShouldHaveAttribtesForPostRequestAntiForgeryTokenAndActionName() =>
            MyController<ProblemController>
            .Instance()
            .WithData(ProblemTestData.GetEntity())
            .Calling(c => c.DeletePost(1))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .ValidatingAntiForgeryToken()
                .ChangingActionNameTo(nameof(ProblemController.Delete)));

        [Fact]
        public void DeletePost_WithProblemWithTheSameIdInTheDb_ShouldDeleteTheProblemAndAllSubmissionsAndReturnRedirectResult()
        {
            Problem problem = ProblemTestData.GetEntity();
            problem.Submissions = SubmissionTestData.GenerateSubmissions().ToList();

            MyController<ProblemController>
           .Instance()
           .WithData(problem)
           .Calling(c => c.DeletePost(problem.Id))
           .ShouldHave()
           .Data(data => data
                .WithSet<Problem>(set =>
                {
                    bool problemExists = set.Any(c => c.Id == problem.Id);
                    Assert.False(problemExists);
                })
                .WithSet<Submission>(set => 
                {
                    bool prolemSubmissionsExist = set.Any(x => x.ProblemId == problem.Id);
                    Assert.False(prolemSubmissionsExist);

                }))
            .AndAlso()
            .ShouldReturn()
            .Redirect(result => result.To<ProblemController>(c => c.All(problem.Lesson.Id)));
        }
    }
}
