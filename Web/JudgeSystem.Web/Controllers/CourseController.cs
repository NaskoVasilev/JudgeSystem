using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.Infrastructure.Extensions;
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

		public IActionResult All()
		{
			var courses = courseService.All();
			ViewData["lessonTypes"] = EnumExtensions.GetEnumValuesAsString<LessonType>();
			return View(courses);
		}

		public IActionResult Lessons(int courseId, string lessonType)
		{
			var lessons = lessonService.LessonsByType(lessonType);
			string courseName = courseService.GetName(courseId);
			ViewData["course"] = courseName + " - " + lessonType;
			return View(lessons);
		}
	}
}
