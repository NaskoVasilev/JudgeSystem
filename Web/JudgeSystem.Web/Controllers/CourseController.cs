using JudgeSystem.Services.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JudgeSystem.Web.Controllers
{
	public class CourseController : Controller
	{
		private readonly ICourseService courseService;

		public CourseController(ICourseService courseService)
		{
			this.courseService = courseService;
		}

		public IActionResult Lessons(int courseId, string lessonType)
		{
			return Content(courseId.ToString() + ' ' + lessonType);
		}
	}
}
