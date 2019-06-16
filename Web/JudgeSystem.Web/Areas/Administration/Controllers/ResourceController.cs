namespace JudgeSystem.Web.Areas.Administration.Controllers
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	using JudgeSystem.Common;
	using JudgeSystem.Data.Models;
	using JudgeSystem.Services.Data;
	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Web.Utilites;
	using JudgeSystem.Web.ViewModels.Resource;
	using JudgeSystem.Web.InputModels.Resource;
	using JudgeSystem.Web.Infrastructure.Extensions;

	using Microsoft.AspNetCore.Mvc;

	public class ResourceController : AdministrationBaseController
	{
		private readonly IResourceService resourceService;
		private readonly IFileManager fileManager;

		public ResourceController(IResourceService resourceService, IFileManager fileManager)
		{
			this.resourceService = resourceService;
			this.fileManager = fileManager;
		}

		public IActionResult Create()
		{
			ViewData[GlobalConstants.ResourceTypesKey] = Utility.GetResourceTypesSelectList();
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(ResourceInputModel model)
		{
			if (!ModelState.IsValid)
			{
				ViewData[GlobalConstants.ResourceTypesKey] = Utility.GetResourceTypesSelectList();

				return View(model);
			}

			string fileName = fileManager.GenerateFileName(model.File);
			await resourceService.CreateResource(model, fileName);
			await fileManager.UploadFile(model.File, fileName);

			return RedirectToAction("Details", "Lesson", new { id = model.LessonId });
		}


		public IActionResult LessonResources(int lessonId)
		{
			IEnumerable<ResourceViewModel> resources = resourceService.LessonResources(lessonId);

			return View(resources);
		}

		public async Task<IActionResult> Edit(int id)
		{
			Resource resource = await resourceService.GetById(id);

			if(resource == null)
			{
				this.ThrowEntityNullException(nameof(resource));
			}

			ViewData[GlobalConstants.ResourceTypesKey] = Utility.GetResourceTypesSelectList();
			ResourceEditInputModel model = resource.To<Resource, ResourceEditInputModel>();
			model.Name = model.Name.NormalizeFileName();
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(ResourceEditInputModel model)
		{
			if (!ModelState.IsValid)
			{
				ViewData[GlobalConstants.ResourceTypesKey] = Utility.GetResourceTypesSelectList();
				return View(model);
			}

			Resource resource = await resourceService.GetById(model.Id);
			if(resource == null)
			{
				this.ThrowEntityNullException(nameof(resource));
			}

			string fileName = string.Empty;
			if(model.File != null)
			{
				fileName = fileManager.GenerateFileName(model.File);
				await fileManager.UploadFile(model.File, fileName);
			}

			await resourceService.Update(model, fileName);
			return RedirectToAction(nameof(LessonResources), "Resource", new { resource.LessonId });
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			Resource resource = await resourceService.GetById(id);
			if(resource == null)
			{
				return BadRequest(string.Format(ErrorMessages.NotFoundEntityMessage, nameof(resource)));
			}

			fileManager.DeleteFile(resource.Link);
			await resourceService.Delete(resource);

			return Content(string.Format(InfoMessages.SuccessfullyDeletedMessage, resource.Name));
		}
	}
}