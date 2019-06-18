using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services.Data;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.InputModels.Contest;
using JudgeSystem.Web.Utilites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace JudgeSystem.Web.Areas.Administration.Controllers
{
	public class ContestController : AdministrationBaseController
	{
		private readonly IContestService contestService;
		private readonly ICourseService courseService;
		private readonly ILessonService lessonService;

		public ContestController(IContestService contestService, ICourseService courseService, ILessonService lessonService)
		{
			this.contestService = contestService;
			this.courseService = courseService;
			this.lessonService = lessonService;
		}

		public IActionResult Create()
		{
			var courses = courseService.GetAllCourses().Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
			ViewData["courses"] = courses;
			ViewData["lessonTypes"] = Utility.GetSelectListItems<LessonType>();
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(ContestCreateInputModel model)
		{
			if(!ModelState.IsValid)
			{
				var courses = courseService.GetAllCourses().Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
				ViewData["courses"] = courses;
				ViewData["lessonTypes"] = Utility.GetSelectListItems<LessonType>();
				return View(model);
			}

			Contest contest = model.To<ContestCreateInputModel, Contest>();
			await contestService.Create(contest);
			return Redirect("/");
		}

		public IActionResult GetLessons(int courseId, LessonType lessonType)
		{
			var lessons = lessonService.GetCourseLesosns(courseId, lessonType);
			return Json(lessons);
		}
	}
}
