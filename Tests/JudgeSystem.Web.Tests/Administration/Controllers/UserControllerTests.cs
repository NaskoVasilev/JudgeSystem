using System.Collections.Generic;
using System.Linq;

using JudgeSystem.Data.Models;
using JudgeSystem.Web.Areas.Administration.Controllers;
using JudgeSystem.Web.Tests.TestData;
using JudgeSystem.Web.ViewModels.User;

using MyTested.AspNetCore.Mvc;
using Xunit;

namespace JudgeSystem.Web.Tests.Administration.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public void Results_WithValidUserId_ShoultReturnUserContestAndPracticeResults()
        {
            ApplicationUser user = TestApplicationUser.GetDefaultUser();
            user.UserPractices = UserPracticeTestData.GetEntities().ToList();
            user.UserContests = UserContestTestData.GetEntities().ToList();
            Practice practice = user.UserPractices.First().Practice;
            Contest contest = user.UserContests.First().Contest;

            MyController<UserController>
            .Instance()
            .WithData(user, practice, contest)
            .Calling(c => c.Results(user.Id))
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<UserResultsViewModel>()
                .Passing(model =>
                {
                    Assert.Equal(user.Id, model.UserId);
                    Assert.Single(model.PracticeResults);
                    Assert.Single(model.ContestResults);

                    UserPracticeResultViewModel practiceResult = model.PracticeResults.First();
                    Assert.Equal(19, practiceResult.ActualPoints);
                    Assert.Equal(practice.Lesson.Id, practiceResult.LessonId);
                    Assert.Equal(practice.Lesson.Name, practiceResult.LessonName);
                    Assert.Equal(100, practiceResult.MaxPoints);
                    Assert.Equal(practice.Id, practiceResult.PracticeId);

                    UserCompeteResultViewModel contestResult = model.ContestResults.First();
                    Assert.Equal(39, contestResult.ActualPoints);
                    Assert.Equal(contest.Lesson.Id, contestResult.LessonId);
                    Assert.Equal(contest.Name, contestResult.ContestName);
                    Assert.Equal(100, contestResult.MaxPoints);
                    Assert.Equal(contest.Id, contestResult.ContestId);
                }));
        }

        [Fact]
        public static void All_WithTenUsers_ShouldReturnViewWithTenUsers()
        {
            var users = ApplicationUserTestData.GenerateUsers().ToList();

            MyController<UserController>
            .Instance()
            .WithData(users)
            .Calling(c => c.All())
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<IEnumerable<UserViewModel>>()
                .Passing(model =>
                {
                    var actualUsers = model.OrderBy(u => u.Username).ToList();
                    Assert.Equal(users.Count, actualUsers.Count);

                    for (int i = 0; i < actualUsers.Count; i++)
                    {
                        Assert.Equal(users[i].UserName, actualUsers[i].Username);
                        Assert.Equal(users[i].Surname, actualUsers[i].Surname);
                        Assert.Equal(users[i].Name, actualUsers[i].Name);
                        Assert.Equal(users[i].Email, actualUsers[i].Email);
                    }
                }));
        }
    }
}
