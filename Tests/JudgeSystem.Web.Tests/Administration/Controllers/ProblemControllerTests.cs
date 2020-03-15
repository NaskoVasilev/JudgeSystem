using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Web.Areas.Administration.Controllers;
using JudgeSystem.Web.InputModels.Problem;
using JudgeSystem.Web.InputModels.Test;
using JudgeSystem.Web.Tests.TestData;
using JudgeSystem.Web.ViewModels.Problem;

using Microsoft.AspNetCore.Http.Internal;
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
                AllowedTimeInMilliseconds = 150,
                AllowedMinCodeDifferenceInPercentage = 0,
                TimeIntervalBetweenSubmissionInSeconds = 10
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
                    Assert.Equal(inputModel.AllowedMinCodeDifferenceInPercentage, editedProblem.AllowedMinCodeDifferenceInPercentage);
                    Assert.Equal(inputModel.TimeIntervalBetweenSubmissionInSeconds, editedProblem.TimeIntervalBetweenSubmissionInSeconds);
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

        [Fact]
        public void All_WithDataInTheDb_ShouldReturnAllProblems()
        {
            Lesson lesson = LessonTestData.GetEntity();
            IEnumerable<Problem> allProblems = ProblemTestData.GetEntities();
            var expectedProblems = allProblems.Where(x => x.LessonId == lesson.Id).ToList();

            MyController<ProblemController>
           .Instance()
           .WithData(context => context
                .WithSet<Problem>(set => set.AddRange(allProblems))
                .WithSet<Lesson>(set => set.Add(lesson)))
           .Calling(c => c.All(lesson.Id))
           .ShouldReturn()
           .View(result => result
               .WithModelOfType<ProblemAllViewModel>()
               .Passing(model =>
               {
                   var actualProblems = model.Problems.ToList();
                   Assert.Equal(lesson.Id, model.LesosnId);
                   Assert.Equal(lesson.Practice.Id, model.PracticeId);
                   Assert.Equal(expectedProblems.Count, actualProblems.Count());

                   for (int i = 0; i < expectedProblems.Count; i++)
                   {
                       Assert.Equal(expectedProblems[i].Id, actualProblems[i].Id);
                       Assert.Equal(expectedProblems[i].Name, actualProblems[i].Name);
                   }
               }));
        }

        [Fact]
        public void AddTest_WithPassedProblemId_ShouldReturnViewWithLoadedProblemNameAndLessonId()
        {
            Problem problem = ProblemTestData.GetEntity();

            MyController<ProblemController>
            .Instance()
            .WithData(problem)
            .Calling(c => c.AddTest(problem.Id))
            .ShouldReturn()
            .View(result => result.WithModelOfType<TestInputModel>()
                .Passing(model => model.LessonId == problem.Lesson.Id && model.ProblemName == problem.Name));
        }

        [Fact]
        public void AddTest_ShouldHaveAttribtesForPostRequestAndAntiForgeryToken()
        {
            Problem problem = ProblemTestData.GetEntity();

            MyController<ProblemController>
            .Instance()
            .WithData(problem)
            .Calling(c => c.AddTest(new TestInputModel { ProblemId = problem.Id }))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .ValidatingAntiForgeryToken());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void AddTest_WithInvalidInputData_ShouldHaveInvalidModelStateAndReturnViewWithSetLessonId(string output)
        {
            Problem problem = ProblemTestData.GetEntity();
            var inputModel = new TestInputModel
            {
                InputData = "test",
                OutputData = output,
                ProblemId = problem.Id
            };

            MyController<ProblemController>
            .Instance()
            .WithData(problem)
            .Calling(c => c.AddTest(inputModel))
            .ShouldHave()
            .ModelState(modelState => modelState
                .For<TestInputModel>()
                .ContainingErrorFor(m => m.OutputData))
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<TestInputModel>()
                .Passing(model => model.LessonId == problem.Lesson.Id && model.InputData == inputModel.InputData && model.ProblemId == problem.Id));
        }

        [Fact]
        public void AddTest_WithValidInputData_ShouldReturnRedirectResultAndShouldAddTheTest()
        {
            Problem problem = ProblemTestData.GetEntity();
            string infoMessage = string.Format(InfoMessages.AddedTest, problem.Name);
            var inputModel = new TestInputModel
            {
                InputData = "test input",
                OutputData = "output data",
                ProblemId = problem.Id,
            };

            MyController<ProblemController>
            .Instance()
            .WithData(problem)
            .Calling(c => c.AddTest(inputModel))
            .ShouldHave()
            .ValidModelState()
            .AndAlso()
            .ShouldHave()
            .Data(data => data
                .WithSet<Test>(set =>
                {
                    bool testExists = set.Any(t => t.ProblemId == inputModel.ProblemId &&
                    t.InputData == inputModel.InputData && t.OutputData == inputModel.OutputData);

                    Assert.True(testExists);
                }))
            .AndAlso()
            .ShouldHave()
            .TempData(tempData => tempData.ContainingEntry(GlobalConstants.InfoKey, infoMessage))
            .AndAlso()
            .ShouldReturn()
            .RedirectToAction(nameof(ProblemController.AddTest), new { problemId = problem.Id });
        }

        [Fact]
        public static void AddTests_WithPassedProblemId_ShouldReturnViewWithLoadedProblemNameAndLessonId()
        {
            Problem problem = ProblemTestData.GetEntity();

            MyController<ProblemController>
            .Instance()
            .WithData(problem)
            .Calling(c => c.AddTests(problem.Id))
            .ShouldReturn()
            .View(result => result.WithModelOfType<ProblemAddTestsInputModel>()
                .Passing(model => model.LessonId == problem.Lesson.Id && model.ProblemName == problem.Name));
        }

        [Fact]
        public void AddTests_ShouldHaveAttribtesForPostRequestAndAntiForgeryToken()
        {
            Problem problem = ProblemTestData.GetEntity();

            MyController<ProblemController>
            .Instance()
            .WithData(problem)
            .Calling(c => c.AddTests(new ProblemAddTestsInputModel { LessonId = problem.Lesson.Id, ProblemId = problem.Id }))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .ValidatingAntiForgeryToken());
        }

        [Fact]
        public void AddTests_WithInvalidInputData_ShouldHaveInvalidModelStateAndReturnViewWithLessonIdAndProblemName()
        {
            Problem problem = ProblemTestData.GetEntity();
            var inputModel = new ProblemAddTestsInputModel
            {
                Tests = null,
                LessonId = problem.Lesson.Id,
                ProblemId = problem.Id
            };

            MyController<ProblemController>
            .Instance()
            .WithData(problem)
            .Calling(c => c.AddTests(inputModel))
            .ShouldHave()
            .ModelState(modelState => modelState
                .For<ProblemAddTestsInputModel>()
                .ContainingErrorFor(m => m.Tests))
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<ProblemAddTestsInputModel>()
                .Passing(model => model.LessonId == problem.Lesson.Id && model.ProblemName == problem.Name));
        }

        [Theory]
        [InlineData("testjson", TestsImportStrategy.Json)]
        [InlineData("test.zip", TestsImportStrategy.Json)]
        [InlineData("test.json.txt", TestsImportStrategy.Json)]
        [InlineData(null, TestsImportStrategy.Json)]
        [InlineData("", TestsImportStrategy.Json)]
        [InlineData("testzip", TestsImportStrategy.Zip)]
        [InlineData("test.json", TestsImportStrategy.Zip)]
        [InlineData("test.zip.txt", TestsImportStrategy.Zip)]
        [InlineData(null, TestsImportStrategy.Zip)]
        [InlineData("", TestsImportStrategy.Zip)]
        public void AddTests_WithInvalidFileExtension_ShouldHaveInvalidModelStateAndReturnViewWithLessonIdAndProblemName(string fileName, TestsImportStrategy strategy)
        {
            string fileExtension = strategy == TestsImportStrategy.Json ? GlobalConstants.JsonFileExtension : GlobalConstants.ZipFileExtension;
            Problem problem = ProblemTestData.GetEntity();
            var inputModel = new ProblemAddTestsInputModel
            {
                Tests = new FormFile(null, 0, 0, fileName, fileName),
                LessonId = problem.Lesson.Id,
                ProblemId = problem.Id,
                Strategy = strategy
            };

            MyController<ProblemController>
            .Instance()
            .WithData(problem)
            .Calling(c => c.AddTests(inputModel))
            .ShouldHave()
            .ModelState(modelState => modelState
                .ContainingError(string.Empty)
                .Equals(string.Format(ErrorMessages.InvalidFileExtension, fileExtension)))
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<ProblemAddTestsInputModel>()
                .Passing(model => model.LessonId == problem.Lesson.Id && model.ProblemName == problem.Name));
        }

        [Theory]
        [InlineData(@"[{""Input"": ""input"", ""OutputData"": ""output"", ""IsTrialTest"": true}]", "InputData")]
        [InlineData(@"[{""InputData"": ""input"", ""Output"": ""output"", ""IsTrialTest"": true}]", "OutputData")]
        [InlineData(@"[{""InputData"": ""input"", ""OutputData"": ""output"", ""IsTest"": true}]", "IsTrialTest")]
        [InlineData(@"[{""Wrong"": ""input""}]", "InputData, OutputData, IsTrialTest")]
        [InlineData(@"[{}]", "InputData, OutputData, IsTrialTest")]
        public void AddTests_WithJsonWithoutRequiredProperties_ShouldSetErrorsInTheModelStateAndReturnView(string json, string missingProperties)
        {
            string expectedErrorMessage = "Required properties are missing from object: " + missingProperties;
            Problem problem = ProblemTestData.GetEntity();
            byte[] buffer = Encoding.UTF8.GetBytes(json);

            using (var memoryStream = new MemoryStream(buffer))
            {
                var inputModel = new ProblemAddTestsInputModel
                {
                    Tests = new FormFile(memoryStream, 0, buffer.Length, "test", "test.json"),
                    LessonId = problem.Lesson.Id,
                    ProblemId = problem.Id,
                    Strategy = TestsImportStrategy.Json
                };

                MyController<ProblemController>
                .Instance()
                .WithData(problem)
                .Calling(c => c.AddTests(inputModel))
                .ShouldHave()
                .ModelState(modelState => modelState.ContainingError(string.Empty).BeginningWith(expectedErrorMessage))
                .AndAlso()
                .ShouldReturn()
                .View(result => result
                    .WithModelOfType<ProblemAddTestsInputModel>()
                    .Passing(model => model.LessonId == problem.Lesson.Id && model.ProblemName == problem.Name));
            }
        }

        [Theory]
        [InlineData(@"{""InputData"": ""input"", ""OutputData"": ""output"", ""IsTrialTest"": true}")]
        [InlineData(@"[{""InputData"": ""input, ""Output"": ""output"", ""IsTrialTest"": true}]")]
        [InlineData(@"[{""InputData"": ""input, Output: output, ""IsTrialTest"": ""true""}]")]
        [InlineData(@"[{""InputData"": ""input, ""Output"": ""output"", ""IsTrialTest"": ""true""]")]
        [InlineData(@"[{""InputData"": ""input, ""Output"": ""output"", ""IsTrialTest"": ""true""}")]
        [InlineData(@"[{""InputData"" ""input, ""Output"" = ""output"", ""IsTrialTest"": ""true""")]
        public void AddTests_WithFileWhichHasInvalidJsonFormat_ShouldSetErrorsInTheModelStateAndReturnView(string json)
        {
            Problem problem = ProblemTestData.GetEntity();
            byte[] buffer = Encoding.UTF8.GetBytes(json);

            using (var memoryStream = new MemoryStream(buffer))
            {
                var inputModel = new ProblemAddTestsInputModel
                {
                    Tests = new FormFile(memoryStream, 0, buffer.Length, "test", "test.json"),
                    LessonId = problem.Lesson.Id,
                    ProblemId = problem.Id,
                    Strategy = TestsImportStrategy.Json
                };

                MyController<ProblemController>
                .Instance()
                .WithDependencies()
                .WithData(problem)
                .Calling(c => c.AddTests(inputModel))
                .ShouldHave()
                .ModelState(modelState => modelState.ContainingError(string.Empty).Equals(ErrorMessages.InvalidJsonFile))
                .AndAlso()
                .ShouldReturn()
                .View(result => result
                    .WithModelOfType<ProblemAddTestsInputModel>()
                    .Passing(model => model.LessonId == problem.Lesson.Id && model.ProblemName == problem.Name));
            }
        }

        [Theory]
        [InlineData(@"[{""InputData"": ""input"", ""OutputData"": """", ""IsTrialTest"": true}, 
                       {""InputData"": ""input"", ""OutputData"": """", ""IsTrialTest"": true}]")]
        [InlineData(@"[{""InputData"": ""input"", ""OutputData"": """", ""IsTrialTest"": true}, 
                      {""InputData"": ""input"", ""OutputData"": ""valid"", ""IsTrialTest"": true}]")]
        [InlineData(@"[{""InputData"": ""input"", ""OutputData"": """", ""IsTrialTest"": true, ""Test"": false }]")]
        public void AddTests_WithValidJsonFileAndInvalidTests_ShouldSetErrorsInTheModelStateAndReturnView(string json)
        {
            Problem problem = ProblemTestData.GetEntity();
            byte[] buffer = Encoding.UTF8.GetBytes(json);

            using (var memoryStream = new MemoryStream(buffer))
            {
                var inputModel = new ProblemAddTestsInputModel
                {
                    Tests = new FormFile(memoryStream, 0, buffer.Length, "test", "test.json"),
                    LessonId = problem.Lesson.Id,
                    ProblemId = problem.Id,
                    Strategy = TestsImportStrategy.Json
                };

                MyController<ProblemController>
                .Instance()
                .WithData(problem)
                .Calling(c => c.AddTests(inputModel))
                .ShouldHave()
                .ModelState(modelState => modelState.ContainingError(string.Empty).Containing("The OutputData field is required"))
                .AndAlso()
                .ShouldReturn()
                .View(result => result
                    .WithModelOfType<ProblemAddTestsInputModel>()
                    .Passing(model => model.LessonId == problem.Lesson.Id && model.ProblemName == problem.Name));
            }
        }

        [Theory]
        [InlineData(@"[{""InputData"": ""input"", ""OutputData"": 12, ""IsTrialTest"": true}]", "Invalid type. Expected String but got Integer. Path '[0].OutputData'")]
        [InlineData(@"[{""InputData"": 12, ""OutputData"": ""test"", ""IsTrialTest"": ""true""}]", "Invalid type. Expected String but got Integer. Path '[0].InputData'")]
        [InlineData(@"[{""InputData"": null, ""OutputData"": ""test"", ""IsTrialTest"": true}]", "Invalid type. Expected String but got Null. Path '[0].InputData'")]
        [InlineData(@"[{""InputData"": ""in"", ""OutputData"": ""test"", ""IsTrialTest"": 15}]", "Invalid type. Expected Boolean but got Integer. Path '[0].IsTrialTest'")]
        [InlineData(@"[{""InputData"": 13.5, ""OutputData"": { ""test"": ""test value""}, ""IsTrialTest"": []}]", "Invalid type. Expected String but got Object. Path '[0].OutputData'")]
        public void AddTests_WithValidJsonAndDifferentPropertyTypes_ShouldSetErrorsInTheModelStateAndReturnView(string json, string expectedError)
        {
            Problem problem = ProblemTestData.GetEntity();
            byte[] buffer = Encoding.UTF8.GetBytes(json);

            using (var memoryStream = new MemoryStream(buffer))
            {
                var inputModel = new ProblemAddTestsInputModel
                {
                    Tests = new FormFile(memoryStream, 0, buffer.Length, "test", "test.json"),
                    LessonId = problem.Lesson.Id,
                    ProblemId = problem.Id,
                    Strategy = TestsImportStrategy.Json
                };

                MyController<ProblemController>
                .Instance()
                .WithData(problem)
                .Calling(c => c.AddTests(inputModel))
                .ShouldHave()
                .ModelState(modelState => modelState.ContainingError(string.Empty).BeginningWith(expectedError))
                .AndAlso()
                .ShouldReturn()
                .View(result => result
                    .WithModelOfType<ProblemAddTestsInputModel>()
                    .Passing(model => model.LessonId == problem.Lesson.Id && model.ProblemName == problem.Name));
            }
        }

        [Theory]
        [InlineData(@"[{""InputData"": ""input0"", ""OutputData"": ""output0"", ""IsTrialTest"": true}, 
                       {""InputData"": ""input1"", ""OutputData"": ""output1"", ""IsTrialTest"": true}]", 2)]
        [InlineData(@"[{""InputData"": ""input0"", ""OutputData"": ""output0"", ""IsTrialTest"": true}]", 1)]
        [InlineData(@"[{""InputData"": ""input0"", ""OutputData"": ""output0"", ""IsTrialTest"": true, ""NotRequiredProperty"": ""not required data""}]", 1)]
        [InlineData("[]", 0)]
        public void AddTests_WithValidJsonAndValidTests_ShouldAddTestsToTheDbAndRedirectToAllTests(string json, int expectedCount)
        {
            Problem problem = ProblemTestData.GetEntity();
            byte[] buffer = Encoding.UTF8.GetBytes(json);

            using (var memoryStream = new MemoryStream(buffer))
            {
                var inputModel = new ProblemAddTestsInputModel
                {
                    Tests = new FormFile(memoryStream, 0, buffer.Length, "test", "test.json"),
                    LessonId = problem.Lesson.Id,
                    ProblemId = problem.Id,
                    Strategy = TestsImportStrategy.Json
                };

                MyController<ProblemController>
                .Instance()
                .WithData(problem)
                .Calling(c => c.AddTests(inputModel))
                .ShouldHave()
                .ValidModelState()
                .AndAlso()
                .ShouldHave()
                .Data(data => data.WithSet<Test>(set =>
                {
                    Assert.Equal(expectedCount, set.Count());
                    var tests = set.ToList();

                    for (int i = 0; i < expectedCount; i++)
                    {
                        Test currentTest = tests[i];
                        Assert.Equal($"input{i}", currentTest.InputData);
                        Assert.Equal($"output{i}", currentTest.OutputData);
                        Assert.True(currentTest.IsTrialTest);
                    }
                }))
                .AndAlso()
                .ShouldReturn()
                .RedirectToAction(nameof(TestController.ProblemTests), new { inputModel.ProblemId });
            }
        }
    }
}
