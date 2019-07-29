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
    using JudgeSystem.Web.Filters;

    public class ResourceController : AdministrationBaseController
	{
		private readonly IResourceService resourceService;
		private readonly IFileManager fileManager;
        private readonly ILessonService lessonService;

        public ResourceController(IResourceService resourceService, IFileManager fileManager, ILessonService lessonService)
		{
			this.resourceService = resourceService;
			this.fileManager = fileManager;
            this.lessonService = lessonService;
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

			return RedirectToAction("Details", "Lesson", new { id = model.LessonId, model.PracticeId });
		}


		public IActionResult LessonResources(int lessonId, int practiceId)
		{
			IEnumerable<ResourceViewModel> resources = resourceService.LessonResources(lessonId);
            var model = new AllResourcesViewModel { Resources = resources, LessonId = lessonId, PracticeId = practiceId };
            return View(model);
		}

		public async Task<IActionResult> Edit(int id)
		{
			Resource resource = await resourceService.GetById(id);

			if(resource == null)
			{
				this.ThrowEntityNotFoundException(nameof(resource));
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
				this.ThrowEntityNotFoundException(nameof(resource));
			}

			string fileName = string.Empty;
			if(model.File != null)
			{
				fileName = fileManager.GenerateFileName(model.File);
				await fileManager.UploadFile(model.File, fileName);
			}

			await resourceService.Update(model, fileName);
            int practiceId = lessonService.GetPracticeId(resource.LessonId);
			return RedirectToAction(nameof(LessonResources), "Resource", new { lessonId = resource.LessonId, practiceId });
		}

        [EndpointExceptionFilter]
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