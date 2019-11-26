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
            Course course = CourseTestData.GetEntity();
            MyController<CourseController>
            .Instance()
            .WithData(course)
            .Calling(c => c.All())
            .ShouldReturn()
            .View(result => result
            .WithModelOfType<IEnumerable<CourseViewModel>>()
            .Passing(model =>
            {
                CourseViewModel firstModel = model.First();
                Assert.NotNull(firstModel);
                Assert.Equal(course.Name, firstModel.Name);
                Assert.Equal(course.Id, firstModel.Id);
            }));
        }

        [Theory]
        [InlineData("Exam")]
        [InlineData("Homework")]
        [InlineData("Exercise")]
        public void Lessons_WithValidArguments_ShouldReturnViewWithCorrectData(string lessonTypeAsString)
        {
            Lesson lesson = LessonTestData.GetEntity();
            Enum.TryParse(lessonTypeAsString, out LessonType lessonType);
            lesson.Type = lessonType;

            MyController<CourseController>
            .Instance()
            .WithData(lesson)
            .Calling(c => c.Lessons(lesson.Course.Id, lessonTypeAsString))
            .ShouldReturn()
            .View(result => result
            .WithModelOfType<CourseLessonsViewModel>()
            .Passing(model =>
            {

                Assert.Equal($"{lesson.Course.Name} - {lessonType}", model.Name);
                LessonLinkViewModel firstLesson = model.Lessons.First();
                Assert.NotNull(firstLesson);
                Assert.Equal(lesson.Name, firstLesson.Name);
                Assert.Equal(lesson.Id, firstLesson.Id);
            }));
        }
    }
}
