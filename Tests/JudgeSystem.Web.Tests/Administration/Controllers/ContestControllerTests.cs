using System;
using System.Collections.Generic;
using System.Linq;

using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Web.Areas.Administration.Controllers;
using JudgeSystem.Web.Dtos.Lesson;
using JudgeSystem.Web.Dtos.Submission;
using JudgeSystem.Web.Filters;
using JudgeSystem.Web.Infrastructure.Attributes.Validation;
using JudgeSystem.Web.InputModels.Contest;
using JudgeSystem.Web.Tests.TestData;
using JudgeSystem.Web.ViewModels.Contest;

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
        public void Create_WithValidContest_ShouldReturnRedirectAndShoudAddTheContest()
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

        [Theory]
        [InlineData(1, LessonType.Exam, "2, 3")]
        [InlineData(1, LessonType.Exercise, "1")]
        [InlineData(2, LessonType.Exam, "")]
        [InlineData(2, LessonType.Exercise, "4")]
        public void GetLessons_WithValidCourseIdAndLessonType_ShouldReturnCorrectData(int courseId, LessonType lessonType, string expectedIds) =>
            MyController<ContestController>
            .Instance()
            .WithUser("admin", GlobalConstants.AdministratorRoleName)
            .WithData(LessonTestData.GetLessons())
            .Calling(c => c.GetLessons(courseId, lessonType))
            .ShouldReturn()
            .Json(result => result
                .WithModelOfType<IEnumerable<ContestLessonDto>>()
                .Passing(model => string.Join(", ", model.Select(l => l.Id)) == expectedIds));

        [Fact]
        public void ActiveContests_WithActiveAndFinishedContests_ShouldReturnOnlyActiveContests() =>
            MyController<ContestController>
            .Instance()
            .WithUser("admin", GlobalConstants.AdministratorRoleName)
            .WithData(ContestTestData.GetContests())
            .Calling(c => c.ActiveContests())
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<IEnumerable<ContestBreifInfoViewModel>>()
                .Passing(model => string.Join(", ", model.Select(l => l.Id)) == "1, 2"));

        [Fact]
        public void Details_WithValidId_ShoudReturnViewWithCorrectModel()
        {
            Contest contest = ContestTestData.GetEntity();

            MyController<ContestController>
            .Instance()
            .WithUser("admin", GlobalConstants.AdministratorRoleName)
            .WithData(contest)
            .Calling(c => c.Details(1))
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<ContestViewModel>()
                .Passing(model =>
                {
                    Assert.Equal(contest.EndTime, model.EndTime);
                    Assert.Equal(contest.StartTime, model.StartTime);
                    Assert.Equal(contest.Id, model.Id);
                    Assert.Equal(contest.Name, model.Name);
                }));
        }

        [Fact]
        public void Edit_WithValidId_ShoudReturnViewWithCorrectModel()
        {
            Contest contest = ContestTestData.GetEntity();

            MyController<ContestController>
            .Instance()
            .WithUser("admin", GlobalConstants.AdministratorRoleName)
            .WithData(contest)
            .Calling(c => c.Edit(1))
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<ContestEditInputModel>()
                .Passing(model =>
                {
                    Assert.Equal(contest.EndTime, model.EndTime);
                    Assert.Equal(contest.StartTime, model.StartTime);
                    Assert.Equal(contest.Id, model.Id);
                    Assert.Equal(contest.Name, model.Name);
                }));
        }

        [Fact]
        public void Edit_ShouldHaveAttribtesForPostRequestAndAntiForgeryToken() =>
            MyController<ContestController>
            .Instance()
            .Calling(c => c.Edit(With.Default<ContestEditInputModel>()))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .ValidatingAntiForgeryToken());

        [Theory]
        [InlineData(TestConstnts.StringLessThan3Symbols)]
        [InlineData(TestConstnts.StringMoreThan50Symbols)]
        public void Edit_WithInvalidModelState_ShouldReturnViewWithTheSameViewModel(string name) =>
            MyController<ContestController>
            .Instance()
            .Calling(c => c.Edit(new ContestEditInputModel { Name = name }))
            .ShouldHave()
            .InvalidModelState()
            .AndAlso()
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<ContestEditInputModel>()
                .Passing(model => model.Name == name));

        [Fact]
        public void Edit_WithEndTimeAndStartTimeInThePast_ShouldHaveInvalidModelState()
        {
            var inputModel = new ContestEditInputModel
            {
                EndTime = DateTime.Now.AddDays(-2),
                StartTime = DateTime.Now.AddDays(-5),
                Name = TestConstnts.StringLong10Symbols
            };

            MyController<ContestController>
            .Instance()
            .Calling(c => c.Edit(inputModel))
            .ShouldHave()
            .ModelState(modelState =>
            {
                modelState.For<ContestEditInputModel>()
                .ContainingErrorFor(m => m.EndTime)
                .ThatEquals(AfterDateTimeNowAttribute.DefaultMessage)
                .AndAlso()
                .ContainingErrorFor(m => m.StartTime)
                .ThatEquals(AfterDateTimeNowAttribute.DefaultMessage);
            });
        }

        [Fact]
        public void Edit_WithEndTimeBeforeStartTime_ShouldHaveInvalidModelStateWithCorrectErrorMessage()
        {
            var inputModel = new ContestEditInputModel
            {
                EndTime = DateTime.Now.AddDays(2),
                StartTime = DateTime.Now.AddDays(5),
                Name = TestConstnts.StringLong10Symbols
            };

            MyController<ContestController>
            .Instance()
            .Calling(c => c.Edit(inputModel))
            .ShouldHave()
            .ModelState(modelState => modelState
                .For<ContestEditInputModel>()
                .ContainingErrorFor(m => m.EndTime)
                .ThatEquals(ContestEditInputModel.StartEndTimeErrorMessage));
        }

        [Fact]
        public void Edit_WithValidContest_ShouldReturnRedirectAndShoudUpdateTheContest()
        {
            Contest contest = ContestTestData.GetEntity();
            var inputModel = new ContestEditInputModel
            {
                Id = contest.Id,
                StartTime = DateTime.Now.AddDays(10),
                EndTime = DateTime.Now.AddDays(30),
                Name = "C# OOP edited"
            };

            MyController<ContestController>
            .Instance()
            .WithUser("nasko", GlobalConstants.AdministratorRoleName)
            .WithData(contest)
            .Calling(c => c.Edit(inputModel))
            .ShouldHave()
            .Data(data => data
                .WithSet<Contest>(set =>
                {
                    Contest editedContest = set.FirstOrDefault(c => c.Id == inputModel.Id);
                    Assert.NotNull(editedContest);
                    Assert.Equal(inputModel.Name, editedContest.Name);
                    Assert.Equal(inputModel.EndTime, editedContest.EndTime);
                    Assert.Equal(inputModel.StartTime, editedContest.StartTime);
                }))
            .AndAlso()
            .ShouldReturn()
            .RedirectToAction(nameof(ContestController.ActiveContests));
        }

        [Fact]
        public void Delete_WithValidId_ShoudReturnViewWithCorrectModel()
        {
            Contest contest = ContestTestData.GetEntity();

            MyController<ContestController>
            .Instance()
            .WithUser("admin", GlobalConstants.AdministratorRoleName)
            .WithData(contest)
            .Calling(c => c.Delete(1))
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<ContestViewModel>()
                .Passing(model =>
                {
                    Assert.Equal(contest.EndTime, model.EndTime);
                    Assert.Equal(contest.StartTime, model.StartTime);
                    Assert.Equal(contest.Id, model.Id);
                    Assert.Equal(contest.Name, model.Name);
                }));
        }

        [Fact]
        public void DeletePost_ShouldHaveAttribtesForPostRequestAntiForgeryTokenAndActionName() =>
            MyController<ContestController>
            .Instance()
            .WithData(ContestTestData.GetEntity())
            .Calling(c => c.DeletePost(1))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForHttpMethod(HttpMethod.Post)
                .ValidatingAntiForgeryToken()
                .ChangingActionNameTo(nameof(ContestController.Delete)));

        [Fact]
        public void DeletePost_WithContestInTheDbAndValidContestId_ShouldDeleteTheContestAndReturnRedirectResult()
        {
            Contest contest = ContestTestData.GetEntity();

            MyController<ContestController>
           .Instance()
           .WithUser("admin", GlobalConstants.AdministratorRoleName)
           .WithData(contest)
           .Calling(c => c.DeletePost(contest.Id))
           .ShouldHave()
           .Data(data => data
                .WithSet<Contest>(set =>
                {
                    bool contestExists = set.Any(c => c.Id == contest.Id);
                    Assert.False(contestExists);
                }))
            .AndAlso()
            .ShouldReturn()
            .RedirectToAction(nameof(ContestController.ActiveContests));
        }

        [Theory]
        [InlineData(1, 10)]
        [InlineData(3, 10)]
        [InlineData(4, 6)]
        public void All_WithDataInTheDb_ShouldReturnContestsForCurrentPage(int page, int expectedCount)
        {
            IEnumerable<Contest> allContests = ContestTestData.GenerateContests();
            var contests = allContests
                .OrderByDescending(c => c.StartTime)
                .Skip((page - 1) * GlobalConstants.ContestsPerPage)
                .Take(GlobalConstants.ContestsPerPage)
                .ToList();

            MyController<ContestController>
           .Instance()
           .WithUser("admin", GlobalConstants.AdministratorRoleName)
           .WithData(allContests)
           .Calling(c => c.All(page))
           .ShouldReturn()
           .View(result => result
               .WithModelOfType<ContestAllViewModel>()
               .Passing(model =>
               {
                   Assert.Equal(4, model.NumberOfPages);
                   Assert.Equal(page, model.CurrentPage);
                   Assert.Equal(expectedCount, contests.Count);

                   var actualContests = model.Contests.ToList();

                   for (int i = 0; i < actualContests.Count; i++)
                   {
                       Contest expectedContest = contests[i];
                       ContestViewModel actualContest = actualContests[i];
                       Assert.Equal(expectedContest.Id, actualContest.Id);
                       Assert.Equal(expectedContest.Name, actualContest.Name);
                   }
               }));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(null)]
        public void Submissions_WithValidArgumnets_ShouldReturnViewWithCorrectData(int? problemId)
        {
            Lesson lesson = LessonTestData.GetEntity();
            Problem problem = ProblemTestData.GetEntity();
            Contest contest = ContestTestData.GetEntity();
            Submission submission = SubmissionTestData.GetEntity();
            ExecutedTest executedTest = ExecutedTestTestData.GetEntity();
            ApplicationUser user = TestApplicationUser.GetDefaultUser();

            MyController<ContestController>
            .Instance()
            .WithUser("admin", GlobalConstants.AdministratorRoleName)
            .WithData(user, lesson, problem, contest, submission, executedTest)
            .Calling(c => c.Submissions(user.Id, contest.Id, problemId, 1))
            .ShouldReturn()
            .View(result => result
            .WithModelOfType<ContestSubmissionsViewModel>()
            .Passing(model =>
            {
                model.ContestName = contest.Name;
                model.ProblemName = problem.Name;
                model.UserId = TestUser.Identifier;
                model.Submissions.ShouldNotBeEmpty();
                SubmissionResult submission = model.Submissions.First();
                submission.Id.ShouldBe(submission.Id);
                submission.ActualPoints.ShouldBe(submission.ActualPoints);
                Assert.Equal("/Administration/Contest/Submissions?contestId=1&userId=TestId&problemId={0}", model.UrlPlaceholder);
                Assert.Equal("/Administration/Contest/Submissions?contestId=1&userId=TestId&problemId=1&page={0}", model.PaginationData.Url);
            }));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(4)]
        public void Submissions_WithValidArgumnets_ShouldReturnViewAndSubmissionsForThePage(int page)
        {
            Problem problem = ProblemTestData.GetEntity();
            Contest contest = ContestTestData.GetEntity();
            ICollection<Submission> submissions = SubmissionTestData.GenerateSubmissions().ToList();
            contest.Submissions = submissions;
            ApplicationUser user = TestApplicationUser.GetDefaultUser();
            var userSidmissions = submissions
                .Where(s => s.ContestId == contest.Id && s.UserId == TestApplicationUser.Id && s.ProblemId == problem.Id)
                .Skip((page - 1) * GlobalConstants.SubmissionsPerPage)
                .Take(GlobalConstants.SubmissionsPerPage)
                .OrderByDescending(s => s.SubmisionDate)
                .ToList();

            MyController<ContestController>
            .Instance()
            .WithUser("admin", GlobalConstants.AdministratorRoleName)
            .WithData(problem, contest)
            .Calling(c => c.Submissions(user.Id, contest.Id, problem.Id, page))
            .ShouldReturn()
            .View(result => result
            .WithModelOfType<ContestSubmissionsViewModel>()
            .Passing(model =>
            {
                var actualSubmisisons = model.Submissions.ToList();
                Assert.Equal(userSidmissions.Count, actualSubmisisons.Count);

                for (int i = 0; i < actualSubmisisons.Count; i++)
                {
                    SubmissionResult actualSubmission = actualSubmisisons[i];
                    Submission expectedSubmission = userSidmissions[i];

                    Assert.Equal(expectedSubmission.Id, actualSubmission.Id);
                    Assert.Equal(expectedSubmission.ActualPoints, actualSubmission.ActualPoints);
                }
            }));
        }

        [Fact]
        public void GetContestResultPagesCount_WithValidContestId_ShoudReturnPagesCount()
        {
            Contest contest = ContestTestData.GetEntity();
            contest.UserContests = UserContestTestData.GenerateUserContests().ToList();

            MyController<ContestController>
            .Instance()
            .WithUser("admin", GlobalConstants.AdministratorRoleName)
            .WithData(contest)
            .Calling(c => c.GetContestResultPagesCount(contest.Id))
            .ShouldHave()
            .ActionAttributes(attributes => attributes.ContainingAttributeOfType<EndpointExceptionFilter>())
            .AndAlso()
            .ShouldReturn()
            .Result(3);
        }
    }
}
