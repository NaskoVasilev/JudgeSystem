using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;

using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Web.Controllers;
using JudgeSystem.Web.Dtos.Submission;
using JudgeSystem.Web.Filters;
using JudgeSystem.Web.InputModels.Submission;
using JudgeSystem.Web.Tests.TestData;
using JudgeSystem.Web.ViewModels.Submission;
using JudgeSystem.Workers.Common;
using JudgeSystem.Web.ViewModels.ExecutedTest;

using Microsoft.AspNetCore.Http;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace JudgeSystem.Web.Tests.Controllers
{
    public class SubmissionControllerTests
    {
        [Fact]
        public void SubmissionControllerActions_ShouldBeAllowedOnlyForAuthorizedUsers() =>
           MyController<SubmissionController>
           .Instance()
           .ShouldHave()
                .Attributes(attributes => attributes.RestrictingForAuthorizedRequests());

        [Fact]
        public void Details_WithValidSubmissionId_ShouldReturnViewWithCorrectData()
        {
            Submission submission = SubmissionTestData.GetEntity();
            ExecutedTest executeTest = ExecutedTestTestData.GetEntity();
            executeTest.IsCorrect = true;

            MyController<SubmissionController>
            .Instance()
            .WithData(submission, executeTest)
            .Calling(c => c.Details(submission.Id))
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<SubmissionViewModel>()
                .Passing(model =>
                {
                    Assert.Equal(submission.Id, model.Id);
                    Assert.Equal(submission.Problem.Name, model.ProblemName);
                    Assert.Equal(SubmissionTestData.Code, model.Code);
                    Assert.Single(model.ExecutedTests);

                    ExecutedTestViewModel actualExecutedTest = model.ExecutedTests.First();
                    Assert.Equal(executeTest.Id, actualExecutedTest.Id);
                    Assert.Equal(executeTest.IsCorrect, actualExecutedTest.IsCorrect);
                    Assert.Equal(executeTest.ExecutionResultType, actualExecutedTest.ExecutionResultType);
                    Assert.Equal(executeTest.MemoryUsed, actualExecutedTest.MemoryUsed);
                    Assert.Equal(executeTest.TimeUsed, actualExecutedTest.TimeUsed);
                    Assert.Equal(executeTest.Output, actualExecutedTest.Output);
                    Assert.Equal(executeTest.Test.OutputData, actualExecutedTest.TestOutputData);
                }));
        }

        [Fact]
        public void Download_WithValidSubmissionId_ShouldReturnFile()
        {
            Submission submission = SubmissionTestData.GetEntity();

            MyController<SubmissionController>
            .Instance()
            .WithData(submission)
            .Calling(c => c.Download(submission.Id))
            .ShouldReturn()
            .File(result => result
                .WithDownloadName(submission.Problem.Name + ".zip")
                .WithContents(submission.Code)
                .WithContentType(GlobalConstants.OctetStreamMimeType));
        }

        [Fact]
        public void GetProblemSubmissions_WithContentIdNull_ShouldHaveEndpointExceptionFilterAndReturnAllSubmissions()
        {
            ExecutedTest executedTest = ExecutedTestTestData.GetEntity();
            Submission submission = SubmissionTestData.GetEntity();
            Practice practice = PracticeTestData.GetEntity();
            submission.Contest = null;
            submission.PracticeId = practice.Id;
            
            Submission contestSubmission = SubmissionTestData.GetEntity();
            contestSubmission.Id = 2;
            ExecutedTest executedTestInContest = ExecutedTestTestData.GetEntity();
            executedTestInContest.SubmissionId = contestSubmission.Id;
            executedTestInContest.Id = 2;

            MyController<SubmissionController>
            .Instance()
            .WithData(submission, contestSubmission, executedTest, executedTestInContest)
            .WithUser()
            .Calling(c => c.GetProblemSubmissions(submission.Problem.Id, 1, 5, null))
            .ShouldHave()
            .ActionAttributes(attributes => attributes.ContainingAttributeOfType<EndpointExceptionFilter>())
            .AndAlso()
            .ShouldReturn()
            .Json(result => result
                .WithModelOfType<IEnumerable<SubmissionResult>>()
                .Passing(model =>
                {
                    Assert.Equal(2, model.Count());
                    Assert.Contains(model, s => s.Id == submission.Id && s.ActualPoints == submission.ActualPoints);
                    Assert.Contains(model, s => s.Id == contestSubmission.Id && s.ActualPoints == contestSubmission.ActualPoints);
                }));
        }

        [Fact]
        public void GetProblemSubmissions_WithValidContentId_ShouldReturnOnlyContestSubmissions()
        {
            ExecutedTest executedTest = ExecutedTestTestData.GetEntity();
            Submission submission = SubmissionTestData.GetEntity();
            Practice practice = PracticeTestData.GetEntity();
            submission.Contest = null;
            submission.PracticeId = practice.Id;
            
            Submission contestSubmission = SubmissionTestData.GetEntity();
            contestSubmission.Id = 2;
            ExecutedTest executedTestInContest = ExecutedTestTestData.GetEntity();
            executedTestInContest.SubmissionId = contestSubmission.Id;
            executedTestInContest.Id = 2;

            MyController<SubmissionController>
            .Instance()
            .WithData(submission, contestSubmission, executedTest, executedTestInContest)
            .WithUser()
            .Calling(c => c.GetProblemSubmissions(submission.Problem.Id, 1, 5, contestSubmission.Contest.Id))
            .ShouldReturn()
            .Json(result => result
                .WithModelOfType<IEnumerable<SubmissionResult>>()
                .Passing(model =>
                {
                    Assert.Single(model);
                    SubmissionResult expectedSubmission = model.First();
                    Assert.Equal(expectedSubmission.Id, contestSubmission.Id);
                    Assert.Equal(expectedSubmission.ActualPoints, contestSubmission.ActualPoints);
                }));
        }

        [Fact]
        public void GetSubmissionsCount_WithContestIdNull_ShouldHaveEndpointExceptionFilterAndReturnCountOfAllSubmissions()
        {
            Submission submission = SubmissionTestData.GetEntity();
            Practice practice = PracticeTestData.GetEntity();
            submission.Contest = null;
            submission.PracticeId = practice.Id;
            Submission contestSubmission = SubmissionTestData.GetEntity();
            contestSubmission.Id = 2;

            MyController<SubmissionController>
            .Instance()
            .WithData(submission, contestSubmission)
            .WithUser()
            .Calling(c => c.GetSubmissionsCount(submission.Problem.Id, null))
            .ShouldHave()
            .ActionAttributes(attributes => attributes.ContainingAttributeOfType<EndpointExceptionFilter>())
            .AndAlso()
            .ShouldReturn()
            .Json(result => result
                .WithModelOfType<int>()
                .Passing(model =>
                {
                    Assert.Equal(2, model);
                }));
        }

        [Fact]
        public void GetSubmissionsCount_WithValidContentId_ShouldReturnCountOfOnlyContestSubmissions()
        {
            Submission submission = SubmissionTestData.GetEntity();
            Practice practice = PracticeTestData.GetEntity();
            submission.Contest = null;
            submission.PracticeId = practice.Id;
            Submission contestSubmission = SubmissionTestData.GetEntity();
            contestSubmission.Id = 2;

            MyController<SubmissionController>
            .Instance()
            .WithData(submission, contestSubmission)
            .WithUser()
            .Calling(c => c.GetSubmissionsCount(submission.Problem.Id, contestSubmission.Contest.Id))
            .ShouldReturn()
            .Json(result => result
                .WithModelOfType<int>()
                .Passing(model =>
                {
                    Assert.Equal(1, model);
                }));
        }

        [Theory]
        [InlineData(1, 5)]
        [InlineData(-5, -2)]
        public void Create_WithContestIdAndNotActiveContest_ShouldReturnBadRquestWithCorrectMessage(int startTime, int endTime)
        {
            var contest = new Contest
            {
                Id = 1,
                StartTime = DateTime.Now.AddDays(startTime),
                EndTime = DateTime.Now.AddDays(endTime),
            };

            MyController<SubmissionController>
            .Instance()
            .WithData(contest)
            .Calling(c => c.Create(new SubmissionInputModel { ContestId = contest.Id }))
            .ShouldReturn()
            .BadRequest(ErrorMessages.ContestIsNotActive);
        }

        [Fact]
        public void Create_WithValidCodeAndPracticeId_ShouldAddSubmissionToTheDbCalculatePointsSetUserIdCompileAndExecuteSubmission()
        {
            Practice practice = PracticeTestData.GetEntity();
            Problem problem = ProblemTestData.GetEntity();
            Test test = TestTestData.GetEntity();
            ConfigureTest(test);
            SubmissionInputModel input = BuildSubmissionInputModel(problem);
            input.PracticeId = practice.Id;

            MyController<SubmissionController>
            .Instance()
            .WithData(problem, test)
            .WithUser()
            .Calling(c => c.Create(input))
            .ShouldHave()
            .Data(data => data.
                WithSet<Submission>(set =>
                {
                    Submission submission = set.First();
                    Assert.Equal(TestApplicationUser.Id, submission.UserId);
                    Assert.Equal(problem.MaxPoints, submission.ActualPoints);
                    Assert.True(submission.CompilationErrors == null || submission.CompilationErrors.Length == 0);
                    Assert.Equal(practice.Id, submission.Practice.Id);
                })
                .WithSet<ExecutedTest>(set =>
                {
                    ExecutedTest executedTest = set.First();
                    Assert.True(string.IsNullOrEmpty(executedTest.Error));
                    Assert.Equal(TestExecutionResultType.Success, executedTest.ExecutionResultType);
                    Assert.True(executedTest.IsCorrect);
                    Assert.Equal(test.OutputData, executedTest.Output);
                    Assert.Equal(test.Id, executedTest.TestId);
                }));
        }

        [Fact]
        public void Create_WithValidData_ShouldHaveAttributesAndReturnJsonResult()
        {
            Contest contest = ContestTestData.GetEntity();
            Problem problem = ProblemTestData.GetEntity();
            Test test = TestTestData.GetEntity();
            ConfigureTest(test);
            SubmissionInputModel input = BuildSubmissionInputModel(problem);
            input.ContestId = contest.Id;

            MyController<SubmissionController>
            .Instance()
            .WithUser()
            .WithHttpContext(CreateDefaultHttpContextWithRemoteIpAddress(TestConstnts.DefaultIpAddress))
            .WithData(problem, contest, test)
            .Calling(c => c.Create(input))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .ContainingAttributeOfType<EndpointExceptionFilter>())
            .AndAlso()
            .ShouldHave()
            .Data(data => data.WithSet<Submission>(set => 
            {
                Submission submission = set.First();
                Assert.Equal(contest.Id, submission.ContestId);
            }))
            .AndAlso()
            .ShouldReturn()
            .Json(result => result
                .WithModelOfType<SubmissionResult>()
                .Passing(model =>
                {
                    Assert.Equal(problem.MaxPoints, model.ActualPoints);
                    Assert.True(model.IsCompiledSuccessfully);
                    Assert.True(model.ExecutedTests.First().IsCorrect);
                }));
        }

        [Fact]
        public void Create_WithInvalidCode_ShouldReturnJsonResultWithCompileTimeError()
        {
            Contest contest = ContestTestData.GetEntity();
            Problem problem = ProblemTestData.GetEntity();
            Test test = TestTestData.GetEntity();
            ConfigureTest(test);
            SubmissionInputModel input = BuildSubmissionInputModel(problem);
            input.ContestId = contest.Id;
            input.Code = "Invalid code format";

            MyController<SubmissionController>
            .Instance()
            .WithUser()
            .WithHttpContext(CreateDefaultHttpContextWithRemoteIpAddress(TestConstnts.DefaultIpAddress))
            .WithData(problem, contest, test)
            .Calling(c => c.Create(input))
            .ShouldHave()
            .Data(data => data.WithSet<Submission>(set => 
            {
                Submission submission = set.First();
                Assert.NotNull(submission.CompilationErrors);
            }))
            .AndAlso()
            .ShouldReturn()
            .Json(result => result
                .WithModelOfType<SubmissionResult>()
                .Passing(model =>
                {
                    Assert.Equal(0, model.ActualPoints);
                    Assert.False(model.IsCompiledSuccessfully);
                    Assert.Empty(model.ExecutedTests);
                }));
        }

        private HttpContext CreateDefaultHttpContextWithRemoteIpAddress(string ipAddress)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Connection.RemoteIpAddress = IPAddress.Parse(ipAddress);
            return httpContext;
        }

        private static SubmissionInputModel BuildSubmissionInputModel(Problem problem) => new SubmissionInputModel()
        {
            Code = "using System;namespace HelloPmgJudge { class Program { static void Main(string[] args) { Console.WriteLine(\"Hello world.\");}}};",
            ProblemId = problem.Id,
            ProgrammingLanguage = ProgrammingLanguage.CSharp
        };

        private static void ConfigureTest(Test test)
        {
            string output = "Hello world.";
            test.OutputData = output;
            test.IsTrialTest = false;
            test.InputData = null;
        }
    }
}
