namespace JudgeSystem.Web.Controllers
{
	using System.Threading.Tasks;

	using JudgeSystem.Services.Data;

	using Microsoft.AspNetCore.Mvc;

	public class LessonController : BaseController
	{
		private readonly ILessonService lessonService;

		public LessonController(ILessonService lessonService)
		{
			this.lessonService = lessonService;
		}

		public async Task<IActionResult> Details(int id)
		{
			//TODO: Check if the contest is locked
			var lesson = await lessonService.GetLessonInfo(id);

			return View(lesson);
		}
	}
}
