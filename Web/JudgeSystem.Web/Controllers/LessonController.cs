namespace JudgeSystem.Web.Controllers
{
	using System.Threading.Tasks;
	using JudgeSystem.Common;
	using JudgeSystem.Services;
	using JudgeSystem.Services.Data;
	using JudgeSystem.Web.ViewModels.Lesson;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Http;

	public class LessonController : BaseController
	{
		private readonly ILessonService lessonService;
		private readonly IPasswordHashService passwordHashService;

		public LessonController(ILessonService lessonService, IPasswordHashService passwordHashService)
		{
			this.lessonService = lessonService;
			this.passwordHashService = passwordHashService;
		}

		[Authorize]
		public async Task<IActionResult> Details(int id)
		{
			var lesson = await lessonService.GetLessonInfo(id);
			string sessionValue = HttpContext.Session.GetString(lesson.Id.ToString());

			if (lesson.IsLocked)
			{
				if (sessionValue != null && sessionValue == this.User.Identity.Name)
				{
					return View(lesson);
				}
				else
				{
					return RedirectToAction(nameof(EnterPassword), new { id = lesson.Id });
				}
			}

			return View(lesson);
		}

		public IActionResult EnterPassword()
		{
			return View();
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> EnterPassword(LessonPasswordInputModel model)
		{
			var lesson = await lessonService.GetById(model.Id);
			if (lesson == null)
			{
				this.ThrowEntityNullException(nameof(lesson));
			}

			if (lesson.LessonPassword == passwordHashService.HashPassword(model.LessonPassword))
			{
				this.HttpContext.Session.SetString(lesson.Id.ToString(), this.User.Identity.Name);
				return RedirectToAction(nameof(Details), new { id = lesson.Id });
			}

			ModelState.AddModelError(string.Empty, ErrorMessages.InvalidLessonPassword);
			return View(model);
		}
	}
}
