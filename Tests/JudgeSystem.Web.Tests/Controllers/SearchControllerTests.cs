using System.Linq;

using JudgeSystem.Common;
using JudgeSystem.Common.Exceptions;
using JudgeSystem.Data.Models;
using JudgeSystem.Web.Controllers;
using JudgeSystem.Web.Tests.TestData;
using JudgeSystem.Web.ViewModels.Search;

using MyTested.AspNetCore.Mvc;
using Xunit;

namespace JudgeSystem.Web.Tests.Controllers
{
    public class SearchControllerTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Results_WithInvalidKeyword_ShouldHaveInvalidModelStateAndReturnView(string keyword) =>
            MyController<SearchController>
            .Instance()
            .Calling(c => c.Results(keyword))
            .ShouldThrow()
            .Exception()
            .OfType<BadRequestException>()
            .WithMessage(ErrorMessages.InvalidSearchKeyword);

        [Fact]
        public void Results_WithValidKeyword_ShouldReturnValidViewWithCorrectData()
        {
            Lesson lesson = LessonTestData.GetEntity();
            lesson.Name = "Data structures";
            Problem problem = ProblemTestData.GetEntity();
            problem.Name = "I love data science";

            MyController<SearchController>
            .Instance()
            .WithData(lesson, problem)
            .Calling(c => c.Results("data"))
            .ShouldReturn()
            .View(result => result
                .WithModelOfType<SearchResultsViewModel>()
                .Passing(model =>
                {
                    SearchLessonViewModel actualLesson = model.Lessons.First();
                    SearchProblemViewModel actualProblem = model.Problems.First();
                    Assert.NotNull(actualLesson);
                    Assert.NotNull(actualProblem);
                    
                    Assert.Equal(lesson.Id, actualLesson.Id);
                    Assert.Equal(lesson.Name, actualLesson.Name);
                    Assert.Equal(lesson.Practice.Id, actualLesson.PracticeId);

                    Assert.Equal(problem.Lesson.Id, actualProblem.LessonId);
                    Assert.Equal(problem.Lesson.Practice.Id, actualProblem.LessonPracticeId);
                    Assert.Equal(problem.Name, actualProblem.Name);
                    Assert.Equal(problem.Lesson.Name, actualProblem.LessonName);
                }));
        }
    }
}
