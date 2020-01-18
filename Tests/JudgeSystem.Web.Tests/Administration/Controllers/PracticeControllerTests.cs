using System.Linq;

using JudgeSystem.Data.Models;
using JudgeSystem.Web.Areas.Administration.Controllers;
using JudgeSystem.Web.Dtos.Submission;
using JudgeSystem.Web.Tests.TestData;
using JudgeSystem.Web.ViewModels.Practice;

using MyTested.AspNetCore.Mvc;
using Xunit;

namespace JudgeSystem.Web.Tests.Administration.Controllers
{
    public class PracticeControllerTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        public static void Submissions_WithValidArguments_ShouldReturnFiltredSubmissions(int? problemId)
        {
            Lesson lesson = LessonTestData.GetEntity();
            Practice practice = PracticeTestData.GetEntity();
            Problem problem = ProblemTestData.GetEntity();
            Submission submission = SubmissionTestData.GetEntity();
            submission.PracticeId = practice.Id;
            ExecutedTest executedTest = ExecutedTestTestData.GetEntity();
            ApplicationUser user = TestApplicationUser.GetDefaultUser();

            MyController<PracticeController>
            .Instance()
            .WithData(user, submission, executedTest)
            .Calling(c => c.Submissions(user.Id, practice.Id, problemId, 1))
            .ShouldReturn()
            .View(result => result
            .WithModelOfType<PracticeSubmissionsViewModel>()
            .Passing(model =>
            {
                Assert.Equal(lesson.Name, model.LessonName);
                Assert.Equal(problem.Name, model.ProblemName);
                Assert.Equal(TestUser.Identifier, model.UserId);
                Assert.NotEmpty(model.Submissions);
                SubmissionResult actualSubmission = model.Submissions.First();
                Assert.Equal(submission.Id, actualSubmission.Id);
                Assert.Equal(submission.ActualPoints, actualSubmission.ActualPoints);
                Assert.Equal("/Administration/Practice/Submissions?practiceId=1&userId=TestId&problemId={0}", model.UrlPlaceholder);
                Assert.Equal("/Administration/Practice/Submissions?practiceId=1&userId=TestId&problemId=1&page={0}", model.PaginationData.Url);
            }));
        }
    }
}
