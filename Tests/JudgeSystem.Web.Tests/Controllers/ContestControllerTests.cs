using System.Collections.Generic;
using System.Linq;

using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Services;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.Controllers;
using JudgeSystem.Web.Dtos.Submission;
using JudgeSystem.Web.Filters;
using JudgeSystem.Web.Resources;
using JudgeSystem.Web.Tests.TestData;
using JudgeSystem.Web.ViewModels.Contest;
using JudgeSystem.Web.ViewModels.Problem;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

using Moq;
using MyTested.AspNetCore.Mvc;
using Shouldly;
using Xunit;

namespace JudgeSystem.Web.Tests.Controllers
{
    public class ContestControllerTests
    {
        [Fact]
        public void ContestControllerActions_ShouldBeAllowedOnlyForAuthorizedUsers() =>
           MyController<ContestController>
           .Instance()
           .ShouldHave()
           .Attributes(attributes => attributes.RestrictingForAuthorizedRequests());

        [Fact]
        public void GetNumberOfPages_ShouldHaveEndpointExceptionFilter() =>
            MyController<ContestController>
            .Instance()
            .Calling(c => c.GetNumberOfPages())
            .ShouldHave()
            .ActionAttributes(attributes => attributes.ContainingAttributeOfType<EndpointExceptionFilter>());

        [Theory]
        [InlineData(0, 0)]
        [InlineData(90, 9)]
        [InlineData(99, 10)]
        [InlineData(11, 2)]
        [InlineData(1, 1)]
        public static void GetNumberOfPages_WithVariousNumberOfContest_ShouldReturnCorrectNumberOfPages(int contestsCount, int expectedNumberOfPages)
        {
            var contests = new List<Contest>();
            for (int i = 0; i < contestsCount; i++)
            {
                contests.Add(new Contest() { Id = i + 1 });
            }

            MyController<ContestController>
            .Instance()
            .WithData(contests)
            .Calling(c => c.GetNumberOfPages())
            .ShouldReturn()
            .Result(expectedNumberOfPages);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        public void MyResults_WithValidArgumnets_ShouldReturnViewWithValidData(int? problemId)
        {
            Lesson lesson = LessonTestData.GetEntity();
            Problem problem = ProblemTestData.GetEntity();
            Contest contest = ContestTestData.GetEntity();
            Submission submission = SubmissionTestData.GetEntity();
            ExecutedTest executedTest = ExecutedTestTestData.GetEntity();

            MyController<ContestController>
            .Instance()
            .WithUser()
            .WithData(lesson, problem, contest, submission, executedTest)
            .Calling(c => c.MyResults(contest.Id, problemId, 1))
            .ShouldReturn()
            .View(result => result
            .WithName("Areas/Administration/Views/Contest/Submissions.cshtml") 
            .WithModelOfType<ContestSubmissionsViewModel>()
            .Passing(model =>
            {
                model.ContestName = contest.Name;
                model.PaginationData.CurrentPage.ShouldBe(1);
                model.PaginationData.Url.ShouldBe("/Contest/MyResults?contestId=1&problemId=1&page={0}");
                model.UrlPlaceholder.ShouldBe("/Contest/MyResults?contestId=1&problemId={0}");
                model.ProblemName = problem.Name;
                model.UserId = TestUser.Identifier;

                model.Submissions.ShouldNotBeEmpty();
                SubmissionResult submission = model.Submissions.First();
                submission.Id.ShouldBe(submission.Id);
                submission.ActualPoints.ShouldBe(submission.ActualPoints);
            }));
        }

        [Fact]
        public void Results_WithContestId_ShouldReturnViewWithValidData()
        {
            Lesson lesson = LessonTestData.GetEntity();
            Problem problem = ProblemTestData.GetEntity();
            Contest contest = ContestTestData.GetEntity();
            Submission submission = SubmissionTestData.GetEntity();
            UserContest userContest = UserContestTestData.GetEntity();
            ApplicationUser user = TestApplicationUser.GetDefaultUser();
            user.Student = StudentTestData.GetEntity();
            string userId = "test user id";
            user.Id = userId;
            submission.UserId = userId;
            userContest.UserId = userId;
            ExecutedTest executedTest = ExecutedTestTestData.GetEntity();

            MyController<ContestController>
            .Instance()
            .WithUser(user => user.InRole(GlobalConstants.StudentRoleName))
            .WithData(lesson, problem, contest, submission, userContest, executedTest, user)
            .Calling(c => c.Results(contest.Id, 1))
            .ShouldReturn()
            .View(result => result
            .WithModelOfType<ContestAllResultsViewModel>()
            .Passing(model =>
            {
                model.Problems.ShouldNotBeEmpty();
                ContestProblemViewModel actualProblem = model.Problems.First();
                actualProblem.Id.ShouldBe(problem.Id);
                actualProblem.Name.ShouldBe(problem.Name);

                model.ContestResults.ShouldNotBeEmpty();
                ContestResultViewModel firstContestResult = model.ContestResults.First();
                firstContestResult.UserId.ShouldBe(userId);
                firstContestResult.Total.ShouldBe(70);
                firstContestResult.Student.FullName.ShouldBe(user.Student.FullName);
                firstContestResult.Student.NumberInCalss.ShouldBe(user.Student.NumberInCalss);

                Assert.True(firstContestResult.PointsByProblem.ContainsKey(problem.Id));
                Assert.True(firstContestResult.PointsByProblem[problem.Id] == submission.ActualPoints);
            }));
        }


        [Fact]
        public void GetContestResultPagesCount_WithValidContestId_ShoudReturnPagesCount()
        {
            Contest contest = ContestTestData.GetEntity();
            contest.UserContests = UserContestTestData.GenerateUserContests().ToList();

            MyController<ContestController>
            .Instance()
            .WithData(contest)
            .Calling(c => c.GetContestResultPagesCount(contest.Id))
            .ShouldHave()
            .ActionAttributes(attributes => attributes.ContainingAttributeOfType<EndpointExceptionFilter>())
            .AndAlso()
            .ShouldReturn()
            .Result(3);
        }

        [Fact]
        public void ExportResults_WithValidId_ShouldReturnFileResult()
        {
            int contestId = 12;
            string problemName = "Sum two numbers";
            byte[] expectedBytes = new byte[] { 255, 45, 155, 20, 6 };
            var columns = new List<string>() { "Номер в клас", "Клас", "Име", problemName, "Общо" };
            var contestResultsTestData = new ContestAllResultsViewModel
            {
                Name = "Test contest",
                Problems = new List<ContestProblemViewModel>
                {
                    new ContestProblemViewModel
                    {
                        Name = problemName
                    }
                }
            };

            var contestServiceMock = new Mock<IContestService>();
            contestServiceMock.Setup(x =>
                x.GetContestReults(contestId, GlobalConstants.DefaultPage, int.MaxValue)).Returns(contestResultsTestData);
            var excelFileGeneratorMock = new Mock<IExcelFileGenerator>();
            excelFileGeneratorMock.Setup(x =>
                x.GenerateContestResultsReport(contestResultsTestData, columns)).Returns(expectedBytes);

            MyController<ContestController>
            .Instance()
            .WithDependencies(
                contestServiceMock.Object,
                excelFileGeneratorMock.Object,
                From.Services<UserManager<ApplicationUser>>(),
                From.Services<IStringLocalizer<SharedResources>>())
            .Calling(c => c.ExportResults(contestId))
            .ShouldReturn()
            .File(expectedBytes, GlobalConstants.OctetStreamMimeType, $"{contestResultsTestData.Name}{GlobalConstants.ExcelFileExtension}");

            contestServiceMock.Verify(x => x.GetContestReults(contestId, GlobalConstants.DefaultPage, int.MaxValue), Times.Once);
            excelFileGeneratorMock.Verify(x => x.GenerateContestResultsReport(contestResultsTestData, columns), Times.Once);
        }
    }
}
