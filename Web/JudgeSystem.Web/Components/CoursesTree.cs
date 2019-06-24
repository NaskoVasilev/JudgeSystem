namespace JudgeSystem.Web.Components
{
    using JudgeSystem.Data.Models.Enums;
    using JudgeSystem.Services.Data;
    using JudgeSystem.Web.Infrastructure.Extensions;

    using Microsoft.AspNetCore.Mvc;

	public class CoursesTree : ViewComponent
	{
		private readonly ICourseService courseService;

		public CoursesTree(ICourseService courseService)
		{
			this.courseService = courseService;
		}

		public IViewComponentResult Invoke()
		{
			var courses = courseService.All();
			ViewData["lessonTypes"] = EnumExtensions.GetEnumValuesAsString<LessonType>();
			return View(courses);
		}
	}
}
