using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.Infrastructure.Extensions;
using JudgeSystem.Web.ViewModels.Lesson;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JudgeSystem.Web.Areas.Administration.Controllers
{
	public class LessonController : AdministrationBaseController
	{
		private readonly IResourceService resourceService;
		private readonly ILessonService lessonService;

		public LessonController(IResourceService resourseService, ILessonService lessonService)
		{
			this.resourceService = resourseService;
			this.lessonService = lessonService;
		}

		public IActionResult Create()
		{
			ViewData["lessonTypes"] = EnumExtensions.GetEnumValuesAsString<LessonType>().
				Select(t => new SelectListItem { Value = t, Text = t })
				.ToList();
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(LessonInputModel model)
		{
			if (!ModelState.IsValid)
			{
				ViewData["lessonTypes"] = EnumExtensions.GetEnumValuesAsString<LessonType>().
				Select(t => new SelectListItem { Value = t, Text = t })
				.ToList();
				return View(model);
			}

			List<Resource> resources = new List<Resource>();

			foreach (var formFile in model.Resources.Where(f => f.Length > 0))
			{
				string fileOriginalName = formFile.FileName;
				var fileName = Path.GetRandomFileName() + fileOriginalName;
				var filePath = GlobalConstants.FileStorePath + fileName;

				Resource resource = resourceService.CreateResource(fileName, fileOriginalName);
				resources.Add(resource);
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await formFile.CopyToAsync(stream);
				}
			}

			//TODO: hash lesson password for more security
			Lesson newLesson = await lessonService.CreateLesson(model, resources);

			return RedirectToAction("Details", "Lesson", new { id = newLesson.Id });
		}
	}
}
