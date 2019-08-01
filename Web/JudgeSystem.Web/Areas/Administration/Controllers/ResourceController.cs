namespace JudgeSystem.Web.Areas.Administration.Controllers
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	using JudgeSystem.Common;
	using JudgeSystem.Data.Models;
	using JudgeSystem.Services.Data;
	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Web.ViewModels.Resource;
	using JudgeSystem.Web.InputModels.Resource;

	using Microsoft.AspNetCore.Mvc;
    using JudgeSystem.Web.Filters;
    using JudgeSystem.Services;

    public class ResourceController : AdministrationBaseController
	{
		private readonly IResourceService resourceService;
        private readonly ILessonService lessonService;
        private readonly IAzureStorageService azureStorageService;
        private readonly IValidationService validationService;

        public ResourceController(
            IResourceService resourceService, 
            ILessonService lessonService,
            IAzureStorageService azureStorageService,
            IValidationService validationService)
		{
			this.resourceService = resourceService;
            this.lessonService = lessonService;
            this.azureStorageService = azureStorageService;
            this.validationService = validationService;
        }

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(ResourceInputModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
            if(!validationService.IsValidFileExtension(model.File.FileName))
            {
                ModelState.AddModelError(string.Empty, 
                    string.Format(ErrorMessages.UnsupportedFileFormat, System.IO.Path.GetExtension(model.File.FileName)));
                return View(model);
            }

            using(var stream = model.File.OpenReadStream())
            {
                string filePath = await azureStorageService.Upload(stream, model.File.FileName, model.Name);
                await resourceService.CreateResource(model, filePath);
            }

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
			ResourceEditInputModel model = resource.To<ResourceEditInputModel>();
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(ResourceEditInputModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
            if (model.File != null && !validationService.IsValidFileExtension(model.File.FileName))
            {
                ModelState.AddModelError(string.Empty,
                    string.Format(ErrorMessages.UnsupportedFileFormat, System.IO.Path.GetExtension(model.File.Name)));
                return View(model);
            }

            Resource resource = await resourceService.GetById(model.Id);

			if(model.File != null)
			{
                using(var stream = model.File.OpenReadStream())
                {
                    string filePath = await azureStorageService.Upload(stream, model.File.FileName, resource.Name);
                    await azureStorageService.Delete(resource.FilePath);
                    await resourceService.Update(model, filePath);
                }
			}
            else
            {
                await resourceService.Update(model);
            }

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

            await azureStorageService.Delete(resource.FilePath);
			await resourceService.Delete(resource);

			return Content(string.Format(InfoMessages.SuccessfullyDeletedMessage, resource.Name));
		}
	}
}