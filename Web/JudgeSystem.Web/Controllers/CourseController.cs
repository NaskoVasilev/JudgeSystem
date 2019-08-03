namespace JudgeSystem.Web.Controllers
{
    using JudgeSystem.Data.Models;
    using JudgeSystem.Data.Models.Enums;
	using JudgeSystem.Services.Data;
    using JudgeSystem.Services.Mapping;
    using JudgeSystem.Web.Infrastructure.Extensions;
    using JudgeSystem.Web.ViewModels.Course;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

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
			ViewData["lessonTypes"] = EnumExtensions.GetEnumValuesAsString<LessonType>();
			var model = courseService.GetById<CourseViewModel>(id);
			return this.View(model);
		}


		public IActionResult All()
		{
			var courses = courseService.All();
			ViewData["lessonTypes"] = EnumExtensions.GetEnumValuesAsString<LessonType>();
			return View(courses);
		}

		public IActionResult Lessons(int courseId, string lessonType)
		{
			var lessons = lessonService.CourseLessonsByType(lessonType, courseId);
			string courseName = courseService.GetName(courseId);
			ViewData["course"] = courseName + " - " + lessonType;
			return View(lessons);
		}
	}
}
