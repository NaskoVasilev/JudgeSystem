using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

		public async Task<IViewComponentResult> InvokeAsync(string className)
		{
			IEnumerable<string> lessonTypes = EnumExtensions.GetEnumValuesAsString<LessonType>();
			var courses = await Task.Run(() => courseService.All());
			ViewData["class"] = className;
			ViewData["lessonTypes"] = lessonTypes; 
			return View(courses);
		}
	}
}
