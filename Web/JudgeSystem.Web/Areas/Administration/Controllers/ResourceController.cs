using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.Infrastructure.Extensions;
using JudgeSystem.Web.ViewModels.Resource;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JudgeSystem.Web.Areas.Administration.Controllers
{
	public class ResourceController : AdministrationBaseController
    {
		private readonly IResourceService resourceService;

		public ResourceController(IResourceService resourceService)
		{
			this.resourceService = resourceService;
		}

		public IActionResult Create()
        {
			ViewData["resourceTypes"] = EnumExtensions.GetEnumValuesAsString<ResourceType>()
				.Select(r => new SelectListItem { Value = r, Text = r.FormatResourceType()});

            return View();
        }

		[HttpPost]
		public async Task<IActionResult> Create(ResourceInputModel model)
		{
			if (!ModelState.IsValid)
			{
				ViewData["resourceTypes"] = EnumExtensions.GetEnumValuesAsString<ResourceType>()
				.Select(r => new SelectListItem { Value = r, Text = r.FormatResourceType() });

				return View(model);
			}

			string fileOriginalName = model.File.FileName;
			var fileName = Path.GetRandomFileName() + fileOriginalName;
			var filePath = GlobalConstants.FileStorePath + fileName;

			await resourceService.CreateResource(model, fileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await model.File.CopyToAsync(stream);
			}

			return RedirectToAction("Details", "Lesson", new { id = model.LessonId });
		}
    }
}