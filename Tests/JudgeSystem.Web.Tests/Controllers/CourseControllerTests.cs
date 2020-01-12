using System;
using System.Collections.Generic;
using System.Linq;

using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Web.Controllers;
using JudgeSystem.Web.Tests.TestData;
using JudgeSystem.Web.ViewModels.Course;
using JudgeSystem.Web.ViewModels.Lesson;

using MyTested.AspNetCore.Mvc;
using Xunit;

namespace JudgeSystem.Web.Tests.Controllers
{
    public class CourseControllerTests
    {
        [Fact]
        public void Details_WithValidId_ShouldReturnViewWithCorrectData()
        {
            Course course = CourseTestData.GetEntity();
            MyController<CourseController>
            .Instance()
            .WithData(course)
            .Calling(c => c.Details(course.Id))
            .ShouldReturn()
            .View(result => result
            .WithModelOfType<CourseViewModel>()
            .Passing(model =>
            {
                Assert.Equal(course.Name, model.Name);
                Assert.Equal(course.Id, model.Id);
            }));
        }

        [Fact]
        public void All_WithDataInTheDb_ShouldReturnViewWithCorrectData()
        {
            var courses = CourseTestData.GenerateCourses().ToList();

            MyController<CourseController>
            .Instance()
            .WithData(courses)
            .Calling(c => c.All())
            .ShouldReturn()
            .View(result => result
            .WithModelOfType<IEnumerable<CourseViewModel>>()
            .Passing(model =>
            {
                var actualCourses = model.ToList();

                for (int i = 0; i < courses.Count; i++)
                {
                    Assert.Equal(courses[i].Name, actualCourses[i].Name);
                    Assert.Equal(courses[i].Id, actualCourses[i].Id);
                }
            }));
        }

        [Theory]
        [InlineData("Exam")]
        [InlineData("Homework")]
        [InlineData("Exercise")]
        [InlineData("WrongType")]
        public void Lessons_WithValidArguments_ShouldReturnViewWithCorrectData(string lessonTypeAsString)
        {
            Course course = CourseTestData.GetEntity();
            IEnumerable<Lesson> lessons = LessonTestData.GenerateLessons();
            var expectedLessons = lessons.Where(x => x.Type.ToString() == lessonTypeAsString && x.CourseId == course.Id).ToList();

            MyController<CourseController>
            .Instance()
            .WithData(data => data
                .WithSet<Lesson>(set => set.AddRange(lessons))
                .WithSet<Course>(set => set.Add(course)))
            .Calling(c => c.Lessons(course.Id, lessonTypeAsString))
            .ShouldReturn()
            .View(result => result
            .WithModelOfType<CourseLessonsViewModel>()
            .Passing(model =>
            {
                Assert.Equal($"{course.Name} - {lessonTypeAsString}", model.Name);

                var actualLessons = model.Lessons.ToList();
                Assert.Equal(expectedLessons.Count, actualLessons.Count);

                for (int i = 0; i < actualLessons.Count; i++)
                {
                    Assert.Equal(expectedLessons[i].Name, actualLessons[i].Name);
                    Assert.Equal(expectedLessons[i].Id, actualLessons[i].Id);
                }
            }));
        }

        [Fact]
        public void Lessons_WithValidArguments_ShouldReturnViewWithCorrectlyMappedData()
        {
            Lesson lesson = LessonTestData.GetEntity();
            Contest activeContest = ContestTestData.GetEntity();
            activeContest.Lesson = lesson;
            
            lesson.Problems.Add(new Problem() { Id = 1 });
            lesson.Problems.Add(new Problem() { Id = 2 });

            var passedContest = new Contest()
            {
                Id = 2,
                EndTime = DateTime.Now.AddDays(-2),
                StartTime = DateTime.Now.AddDays(-1),
                LessonId = lesson.Id
            };

            var futureContest = new Contest()
            {
                Id = 3,
                EndTime = DateTime.Now.AddDays(3),
                StartTime = DateTime.Now.AddDays(1),
                LessonId = lesson.Id
            };

            lesson.Contests.Add(futureContest);
            lesson.Contests.Add(activeContest);
            lesson.Contests.Add(passedContest);

            MyController<CourseController>
            .Instance()
            .WithData(lesson)
            .Calling(c => c.Lessons(lesson.Course.Id, lesson.Type.ToString()))
            .ShouldReturn()
            .View(result => result
            .WithModelOfType<CourseLessonsViewModel>()
            .Passing(model =>
            {
                LessonLinkViewModel actualLesson = model.Lessons.First();

                Assert.Equal(lesson.Id, actualLesson.Id);
                Assert.Equal(lesson.Name, actualLesson.Name);
                Assert.Equal(lesson.Problems.Count, actualLesson.ProblemsCount);
                Assert.Equal(lesson.Practice.Id, actualLesson.PracticeId);
                Assert.Equal(lesson.Type, actualLesson.Type);
                Assert.Single(actualLesson.Contests);
                Assert.Equal(lesson.CourseId, actualLesson.CourseId);

                LessonContestViewModel contest = actualLesson.Contests.First();
                Assert.Equal(activeContest.Name, contest.Name);
                Assert.Equal(activeContest.Id, contest.Id);
            }));
        }
    }
}
