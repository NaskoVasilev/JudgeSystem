using System.Linq;

using JudgeSystem.Data.Models;
using JudgeSystem.Web.Controllers;
using JudgeSystem.Web.Tests.TestData;
using JudgeSystem.Web.ViewModels.User;

using MyTested.AspNetCore.Mvc;
using Xunit;

namespace JudgeSystem.Web.Tests.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public void UserControllerActions_ShouldBeAllowedOnlyForAuthorizedUsers() =>
           MyController<UserController>
           .Instance()
           .ShouldHave()
                .Attributes(attributes => attributes.RestrictingForAuthorizedRequests());
        [Fact]
        public void MyResults_WithNoDataInTheDbForCurrentUser_ShouldReturnViewWithNoData() => MyController<UserController>
            .Instance()
            .WithUser()
            .WithData()
            .Calling(c => c.MyResults())
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<UserResultsViewModel>()
                .Passing(model =>
                {
                    Assert.Empty(model.ContestResults);
                    Assert.Empty(model.PracticeResults);
                }));

        [Fact]
        public void MyResults_WithDataInTheDbForCurrentUser_ShouldReturnViewWithValidData()
        {
            ApplicationUser user = TestApplicationUser.GetDefaultUser();
            Contest contest = ContestTestData.GetEntity();
            Problem problem = ProblemTestData.GetEntity();
            Practice practice = PracticeTestData.GetEntity();
            Submission submission = SubmissionTestData.GetEntity();
            UserContest userContest = UserContestTestData.GetEntity();
            userContest.Contest = null;
            userContest.ContestId = contest.Id;
            var userPractice = new UserPractice { PracticeId = practice.Id, UserId = TestApplicationUser.Id };


            MyController<UserController>
            .Instance()
            .WithUser()
            .WithData(user, submission, userContest, userPractice)
            .Calling(c => c.MyResults())
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<UserResultsViewModel>()
                .Passing(model =>
                {
                    Assert.Single(model.ContestResults);
                    Assert.Single(model.PracticeResults);
                    
                    UserCompeteResultViewModel contestResult = model.ContestResults.First();
                    Assert.Equal(contest.Name, contestResult.ContestName);
                    Assert.Equal(contest.Id, contestResult.ContestId);
                    Assert.Equal(submission.ActualPoints, contestResult.ActualPoints);
                    Assert.Equal(problem.MaxPoints, contestResult.MaxPoints);

                    UserPracticeResultViewModel practiceResult = model.PracticeResults.First();
                    Assert.Equal(problem.Lesson.Name, practiceResult.LessonName);
                    Assert.Equal(problem.Lesson.Id, practiceResult.LessonId);
                    Assert.Equal(0, practiceResult.ActualPoints);
                    Assert.Equal(problem.MaxPoints, practiceResult.MaxPoints);
                }));
        }
    }
}
