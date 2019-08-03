namespace JudgeSystem.Web.Components
{
    using JudgeSystem.Data.Models.Enums;
    using JudgeSystem.Services.Data;
    using JudgeSystem.Web.Infrastructure.Extensions;

    using Microsoft.AspNetCore.Mvc;

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
