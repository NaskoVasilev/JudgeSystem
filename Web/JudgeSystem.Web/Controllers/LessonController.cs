namespace JudgeSystem.Web.Controllers
{
	using System.Threading.Tasks;
	using JudgeSystem.Common;
	using JudgeSystem.Services;
	using JudgeSystem.Services.Data;
	using JudgeSystem.Web.ViewModels.Lesson;
	using JudgeSystem.Web.InputModels.Lesson;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Identity;
	using JudgeSystem.Data.Models;

	public class LessonController : BaseController
	{
		private readonly ILessonService lessonService;
		private readonly IContestService contestService;
		private readonly IPasswordHashService passwordHashService;
		private readonly UserManager<ApplicationUser> userManager;

		public LessonController(ILessonService lessonService, IContestService contestService, 
			IPasswordHashService passwordHashService, UserManager<ApplicationUser> userManager)
		{
			this.lessonService = lessonService;
			this.contestService = contestService;
			this.passwordHashService = passwordHashService;
			this.userManager = userManager;
		}

		[Authorize]
		public async Task<IActionResult> Details(int id, int? contestId)
		{
			var lesson = await lessonService.GetLessonInfo(id);
			lesson.ContestId = contestId;
			if(contestId.HasValue)
			{
				string userId = userManager.GetUserId(this.User);
				await contestService.AddUserToContestIfNotAdded(userId, contestId.Value);
			}

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
