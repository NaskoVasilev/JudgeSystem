using JudgeSystem.Services.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JudgeSystem.Web.Controllers
{
	public class LessonController : BaseController
	{
		private readonly ILessonService lessonService;

		public LessonController(ILessonService lessonService)
		{
			this.lessonService = lessonService;
		}

		public async Task<IActionResult> Details(int id)
		{
			var lesson = await lessonService.GetLessonInfo(id);

			return View(lesson);
		}
	}
}
