using System.Threading.Tasks;
using System.IO;

using JudgeSystem.Services.Data;
using JudgeSystem.Services;
using JudgeSystem.Web.Dtos.Resource;
using JudgeSystem.Common;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Controllers
{
    [Authorize]
	public class ResourceController : BaseController
	{
		private readonly IResourceService resourceService;
        private readonly IFileStorageService fileStorageService;

        public ResourceController(
            IResourceService resourceService,
            IFileStorageService fileStorageService)
		{
			this.resourceService = resourceService;
            this.fileStorageService = fileStorageService;
        }

		public async Task<IActionResult> Download(int id)
		{
            ResourceDto resource = await resourceService.GetById<ResourceDto>(id);
            string mimeType = GlobalConstants.OctetStreamMimeType;

            using(var stream = new MemoryStream())
            {
                await fileStorageService.Download(resource.FilePath, stream);
		    	return File(stream.ToArray(), mimeType, resource.Name + Path.GetExtension(resource.FilePath)); 
            }
		}
	}
}
