using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services.Data;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.Infrastructure.Extensions;
using JudgeSystem.Web.Utilites;
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
			ViewData["lessonTypes"] = Utility.GetSelectListItems<LessonType>();

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

		public async Task<IActionResult> Edit(int id)
		{
			Lesson lesson = await lessonService.GetById(id);
			if(lesson == null)
			{
				string errorMessage = string.Format(GlobalConstants.NotFoundEntityMessage, "lesson");
				return ShowError(errorMessage, "All", "Course");
			}
			var model = lesson.To<Lesson, LessonEditInputModel>();
			ViewData["lessonTypes"] = Utility.GetSelectListItems<LessonType>();
			return View(model);
		}

		public async Task<IActionResult> Edit(LessonEditInputModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			Lesson lesson = await lessonService.GetById(model.Id);
			if(lesson == null)
			{
				string message = string.Format(GlobalConstants.NotFoundEntityMessage, "lesson");
				return ShowError(message, "All", "Course");
			}

			lesson.Name = model.Name;
			lesson.Type = model.Type;
			string errorMessage = string.Empty;

			if (!string.IsNullOrEmpty(model.OldPassword) && 
				model.OldPassword == lesson.LessonPassword && 
				model.LessonPassword.Length > 5 )
			{
				lesson.LessonPassword = model.LessonPassword;

			}
			else if (!string.IsNullOrEmpty(model.OldPassword))
			{
				if(model.OldPassword != lesson.LessonPassword)
				{
					errorMessage = GlobalConstants.DiffrentLessonPasswords;
				}
				else if(model.LessonPassword.Length < 5)
				{
					errorMessage = GlobalConstants.TooShorPasswordMessage;
				}
			}

			if (!string.IsNullOrEmpty(errorMessage))
			{
				ModelState.AddModelError(string.Empty, errorMessage);
				return View(model);
			}
		}
	}
}
