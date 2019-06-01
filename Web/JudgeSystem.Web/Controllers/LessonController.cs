namespace JudgeSystem.Web.Controllers
{
	using System.Threading.Tasks;
	using JudgeSystem.Common;
	using JudgeSystem.Services;
	using JudgeSystem.Services.Data;
	using JudgeSystem.Web.ViewModels.Lesson;

	using Microsoft.AspNetCore.Mvc;

	public class LessonController : BaseController
	{
		private readonly ILessonService lessonService;
		private readonly IPasswordHashService passwordHashService;

		public LessonController(ILessonService lessonService , IPasswordHashService passwordHashService)
		{
			this.lessonService = lessonService;
			this.passwordHashService = passwordHashService;
		}

		public async Task<IActionResult> Details(int id)
		{
			var lesson = await lessonService.GetLessonInfo(id);

			if (lesson.IsLocked)
			{
				return RedirectToAction(nameof(EnterPassword), new { id = lesson.Id });
			}

			return View(lesson);
		}

		public IActionResult EnterPassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> EnterPassword(LessonPasswordInputModel model)
		{
			var lesson = await lessonService.GetById(model.Id);
			if (lesson == null)
			{
				this.ThrowEntityNullException(nameof(lesson));
			}

			if(lesson.LessonPassword == passwordHashService.HashPassword(model.LessonPassword))
			{
				var lessonInfo = await lessonService.GetLessonInfo(lesson.Id);
				return View("Details", lessonInfo);
			}

			ModelState.AddModelError(string.Empty, ErrorMessages.InvalidLessonPassword);
			return View(model);
		}
	}
}
