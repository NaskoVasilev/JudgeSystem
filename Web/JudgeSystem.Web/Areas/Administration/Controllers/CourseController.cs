using JudgeSystem.Services.Data;
using JudgeSystem.Web.ViewModels.Course;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JudgeSystem.Web.Areas.Administration.Controllers
{
	//TODO: Only for admins
	[Area("Administration")]
	public class CourseController : Controller
    {
		private readonly ICourseService courseService;

		public CourseController(ICourseService courseService)
		{
			this.courseService = courseService;
		}

		public IActionResult Create()
        {
            return View();
        }

		[HttpPost]
		public async Task<IActionResult> Create(CourseInputModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			await courseService.Add(model);
			//TODO: Redirect to all courses
			return Json(model);
		}

		public IActionResult AddLesson(int courseId)
		{
			return Content(courseId.ToString());
		}

		public IActionResult Lessons(int courseId, string type)
		{
			return Content(courseId.ToString());
		}
    }
}