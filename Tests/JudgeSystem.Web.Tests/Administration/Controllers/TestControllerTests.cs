using System.IO;
using System.Linq;

using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.Areas.Administration.Controllers;
using JudgeSystem.Web.Filters;
using JudgeSystem.Web.InputModels.Test;
using JudgeSystem.Web.Tests.Mocks;
using JudgeSystem.Web.Tests.TestData;
using JudgeSystem.Web.ViewModels.Test;

using Microsoft.AspNetCore.Hosting;

using MyTested.AspNetCore.Mvc;
using Xunit;

namespace JudgeSystem.Web.Tests.Administration.Controllers
{
    public class TestControllerTests
    {
        [Fact]
        public static void ProblemTests_WithValidArguments_ShouldReturnTestForPassedProblem()
        {
            Problem problem = ProblemTestData.GetEntity();
            int problemId = problem.Id;
            var tests = TestTestData.GetEntities().ToList();
            tests.Reverse();

            MyController<TestController>
           .Instance()
           .WithData(data => data
               .WithSet<Test>(set => set.AddRange(tests))
               .WithSet<Problem>(set => set.Add(problem)))
           .Calling(c => c.ProblemTests(problemId))
           .ShouldReturn()
           .View(result => result
               .WithModelOfType<ProblemTestsViewModel>()
               .Passing(model =>
               {
                   tests.Reverse();
                   var actualTests = model.Tests.ToList();
                   Assert.Equal(problem.Lesson.Id, model.LessonId);
                   Assert.Equal(tests.Count, actualTests.Count());

                   for (int i = 0; i < tests.Count; i++)
                   {
                       Assert.Equal(tests[i].Id, actualTests[i].Id);
                       Assert.Equal(tests[i].IsTrialTest, actualTests[i].IsTrialTest);
                       Assert.Equal(tests[i].InputData, actualTests[i].InputData);
                       Assert.Equal(tests[i].OutputData, actualTests[i].OutputData);
                   }
               }));
        }

        [Fact]
        public void Edit_WithValidId_ShoudReturnViewWithCorrectModel()
        {
            Test test = TestTestData.GetEntity();

            MyController<TestController>
            .Instance()
            .WithData(test)
            .Calling(c => c.Edit(test.Id))
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<TestEditInputModel>()
                .Passing(model => model.Id == test.Id &&
                model.InputData == test.InputData &&
                model.OutputData == test.OutputData &&
                model.IsTrialTest == test.IsTrialTest));
        }

        [Fact]
        public void Edit_ShouldHaveAttribtesForPostRequestAndAntiForgeryToken() =>
            MyController<TestController>
            .Instance()
            .Calling(c => c.Edit(With.Default<TestEditInputModel>()))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .ValidatingAntiForgeryToken());

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Edit_WithInvalidInputData_ShouldHaveInvalidModelStateAndReturnViewWithTheSameViewModel(string outputData)
        {
            var inputModel = new TestEditInputModel
            {
                Id = 1,
                InputData = "test input data",
                ProblemId = 2,
                OutputData = outputData
            };

            MyController<TestController>
            .Instance()
            .Calling(c => c.Edit(inputModel))
            .ShouldHave()
            .ModelState(modelState => modelState
                .For<TestEditInputModel>()
                .ContainingErrorFor(m => m.OutputData))
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<TestEditInputModel>()
                .Passing(model => model.Id == inputModel.Id &&
                model.InputData == inputModel.InputData &&
                model.ProblemId == inputModel.ProblemId));
        }

        [Fact]
        public void Edit_WithValidInput_ShouldReturnRedirectResultAndUpdateTheTest()
        {
            Test test = TestTestData.GetEntity();
            var inputModel = new TestEditInputModel
            {
                ProblemId = test.Problem.Id,
                InputData = "other input data",
                Id = test.Id,
                IsTrialTest = !test.IsTrialTest,
                OutputData = "other output data"
            };

            MyController<TestController>
            .Instance()
            .WithData(test)
            .Calling(c => c.Edit(inputModel))
            .ShouldHave()
            .Data(data => data
                .WithSet<Test>(set =>
                {
                    Test editedTest = set.FirstOrDefault(c => c.Id == inputModel.Id);
                    Assert.NotNull(editedTest);
                    Assert.Equal(inputModel.InputData, editedTest.InputData);
                    Assert.Equal(inputModel.OutputData, editedTest.OutputData);
                    Assert.Equal(inputModel.IsTrialTest, editedTest.IsTrialTest);
                }))
            .AndAlso()
            .ShouldReturn()
            .RedirectToAction(nameof(TestController.ProblemTests), new { problemId = test.Problem.Id });
        }

        [Fact]
        public void Delete_ShouldHaveAttribtesForPostRequestAndEndpointExceptionFilter() =>
            MyController<TestController>
            .Instance()
            .WithData(TestTestData.GetEntity())
            .Calling(c => c.Delete(1))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .ContainingAttributeOfType<EndpointExceptionFilter>());

        [Fact]
        public void Delete_WithValidTestId_ShouldDeleteTheTestAndReturnContentResult()
        {
            Test test = TestTestData.GetEntity();

            MyController<TestController>
           .Instance()
           .WithData(test)
           .Calling(c => c.Delete(test.Id))
           .ShouldHave()
           .Data(data => data
                .WithSet<Test>(set =>
                {
                    bool testExists = set.Any(c => c.Id == test.Id);
                    Assert.False(testExists);
                }))
            .AndAlso()
            .ShouldReturn()
            .Content(string.Format(InfoMessages.SuccessfullyDeletedMessage, "test"));
        }

        [Fact]
        public void DownloadTemplate_ShoudReadTemplateFileAndReturnFileResult()
        {
            IHostingEnvironment env = HostingEnvironmentMock.CreateInstance();
            string filePath = env.WebRootPath + GlobalConstants.AddTestsTemplsteFilePath;
            byte[] bytes = File.ReadAllBytes(filePath);

            MyController<TestController>
            .Instance()
            .WithDependencies(From.Services<ITestService>(), From.Services<IProblemService>(), env)
            .Calling(c => c.DownloadTemplate())
            .ShouldReturn()
            .File(resut => resut
                .WithContents(bytes)
                .WithContentType(GlobalConstants.OctetStreamMimeType)
                .WithDownloadName(GlobalConstants.AddTestsTemplateFileName));
        }
    }
}
