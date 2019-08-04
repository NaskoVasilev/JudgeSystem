using JudgeSystem.Services.Data;

using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Components
{
    [ViewComponent(Name = "CoursesTree")]
	public class CoursesTreeViewComponent : ViewComponent
	{
		private readonly ICourseService courseService;

		public CoursesTreeViewComponent(ICourseService courseService)
		{
			this.courseService = courseService;
		}

		public IViewComponentResult Invoke()
		{
			var courses = courseService.All();
			return View(courses);
		}
	}
}
