namespace JudgeSystem.Web.Areas.Administration.Controllers
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using JudgeSystem.Common;
	using JudgeSystem.Data.Models;
	using JudgeSystem.Data.Models.Enums;
	using JudgeSystem.Services.Data;
	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Web.Infrastructure.Extensions;
	using JudgeSystem.Web.Utilites;
	using JudgeSystem.Web.ViewModels.Lesson;
	using JudgeSystem.Web.InputModels.Lesson;

	using Microsoft.AspNetCore.Mvc.Rendering;
	using Microsoft.AspNetCore.Mvc;
	using JudgeSystem.Services;
    using JudgeSystem.Web.Filters;

    public class LessonController : AdministrationBaseController
	{
		private readonly IResourceService resourceService;
		private readonly ILessonService lessonService;
		private readonly IFileManager fileManager;
		private readonly IPasswordHashService passwordHashService;

		public LessonController(IResourceService resourseService, ILessonService lessonService, 
			IFileManager fileManager, IPasswordHashService passwordHashService)
		{
			this.resourceService = resourseService;
			this.lessonService = lessonService;
			this.fileManager = fileManager;
			this.passwordHashService = passwordHashService;
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
				string fileName = fileManager.GenerateFileName(formFile);
				Resource resource = resourceService.CreateResource(fileName, formFile.FileName);
				resources.Add(resource);
				await fileManager.UploadFile(formFile, fileName);
			}

			if(model.LessonPassword != null)
			{
				model.LessonPassword = passwordHashService.HashPassword(model.LessonPassword);
			}

			Lesson newLesson = await lessonService.CreateLesson(model, resources);

			return RedirectToAction("Details", "Lesson", new { id = newLesson.Id });
		}

		public async Task<IActionResult> Edit(int id, string lessonType, int courseId)
		{
			Lesson lesson = await lessonService.GetById(id);
			if(lesson == null)
			{
				string errorMessage = string.Format(ErrorMessages.NotFoundEntityMessage, "lesson");
				return ShowError(errorMessage, "All", "Course", new { lessonType, courseId});
			}
			var model = lesson.To<Lesson, LessonEditInputModel>();
			ViewData["lessonTypes"] = Utility.GetSelectListItems<LessonType>();
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(LessonEditInputModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			Lesson lesson = await lessonService.GetById(model.Id);
			if(lesson == null)
			{
				string message = string.Format(ErrorMessages.NotFoundEntityMessage, "lesson");
				return ShowError(message, "Lessons", "Course", new { lessonType = lesson.Type, lesson.CourseId });
			}

			lesson.Name = model.Name;
			lesson.Type = model.Type;
			await lessonService.Update(lesson);

			return RedirectToAction("Lessons", "Course", new { lessonType = lesson.Type, lesson.CourseId });
		}

		public IActionResult AddPassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> AddPassword(LessonAddPasswordInputModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			Lesson lesson = await lessonService.GetById(model.Id);
			if(lesson == null)
			{
				this.ThrowEntityNotFoundException(nameof(lesson));
			}

			if (lesson.IsLocked)
			{
				string message = string.Format(ErrorMessages.LockedLesson);
				return ShowError(message, "Lessons", "Course", new { lessonType = lesson.Type, lesson.CourseId });
			}

			lesson.LessonPassword = passwordHashService.HashPassword(model.LessonPassword);
			await lessonService.Update(lesson);

			string infoMessage = string.Format(InfoMessages.AddPasswordSuccessfully, lesson.Name);
			return this.ShowInfo(infoMessage, "Lessons", "Course", new { lessonType = lesson.Type, lesson.CourseId });
		}

		public IActionResult ChangePassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ChangePassword(LessonChangePasswordInputModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			Lesson lesson = await lessonService.GetById(model.Id);
			if(lesson.IsLocked && lesson.LessonPassword == passwordHashService.HashPassword(model.OldPassword))
			{
				lesson.LessonPassword = passwordHashService.HashPassword(model.NewPassword);
				await lessonService.Update(lesson);
				string infoMessage = string.Format(InfoMessages.ChangePasswordSuccessfully, lesson.Name);
				return this.ShowInfo(infoMessage, "Lessons", "Course", new { lessonType = lesson.Type, lesson.CourseId });

			}
			else
			{
				string errorMessage = ErrorMessages.DiffrentLessonPasswords;
				this.ModelState.AddModelError(string.Empty, errorMessage);
				return View(model);
			}
		}

        [EndpointExceptionFilter]
		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			Lesson lesson = await this.lessonService.GetById(id);

			if(lesson == null)
			{
				return BadRequest(string.Format(ErrorMessages.NotFoundEntityMessage, nameof(lesson)));
			}

			string lessonName = lesson.Name;
			await lessonService.Delete(lesson);

			return Content(string.Format(InfoMessages.SuccessfullyDeletedMessage, lessonName));
		}

		public IActionResult RemovePassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> RemovePassword(LessonRemovePasswordInputModel model)
		{
			Lesson lesson = await lessonService.GetById(model.Id);
			if(lesson == null)
			{
				this.ThrowEntityNotFoundException(nameof(lesson));
			}

			if(lesson.LessonPassword == passwordHashService.HashPassword(model.OldPassword))
			{
				lesson.LessonPassword = null;
				await lessonService.Update(lesson);

				string infoMessage = string.Format(InfoMessages.PasswordRemoved, lesson.Name);
				return this.ShowInfo(infoMessage, "Lessons", "Course", new { lessonType = lesson.Type, lesson.CourseId });
			}

			this.ModelState.AddModelError(string.Empty, ErrorMessages.DiffrentLessonPasswords);
			return View();
		}
	}
}
