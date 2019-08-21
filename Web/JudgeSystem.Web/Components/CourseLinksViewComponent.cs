using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.Infrastructure.Extensions;
using JudgeSystem.Web.ViewModels.Course;

using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Components
{
    [ViewComponent(Name = "CourseLinks")]
	public class CourseLinksViewComponent : ViewComponent
	{
		private readonly ICourseService courseService;

		public CourseLinksViewComponent(ICourseService courseService)
		{
			this.courseService = courseService;
		}

		public IViewComponentResult Invoke()
		{
            var model = new AllCoursesViewModel
            {
                LessonTypes = EnumExtensions.GetEnumValuesAsString<LessonType>(),
                Courses = courseService.All()
            };
			return View(model);
		}
	}
}
