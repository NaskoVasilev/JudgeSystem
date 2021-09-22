using System.Collections.Generic;

using JudgeSystem.Services.Data;
using JudgeSystem.Web.ViewModels.Course;
using JudgeSystem.Web.ViewModels.Lesson;
using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Controllers
{
    public class CourseController : BaseController
	{
		private readonly ILessonService lessonService;
		private readonly ICourseService courseService;

		public CourseController(ICourseService courseService, ILessonService lessonService)
		{
			this.courseService = courseService;
			this.lessonService = lessonService;
		}

		public IActionResult Details(int id)
		{
            CourseViewModel model = courseService.GetById<CourseViewModel>(id);
			return View(model);
		}

		public IActionResult All()
		{
            IEnumerable<CourseViewModel> courses = courseService.All();
			return View(courses);
		}

		public IActionResult Lessons(int courseId)
		{
            var model = new CourseLessonsViewModel
            {
                Lessons = lessonService.GetByCourseId<LessonLinkViewModel>(courseId),
                Name = courseService.GetName(courseId),
                CourseId = courseId
            };

			return View(model);
		}
	}
}
