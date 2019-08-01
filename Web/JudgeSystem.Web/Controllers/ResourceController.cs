namespace JudgeSystem.Web.Controllers
{
    using System.Threading.Tasks;

    using JudgeSystem.Data.Models;
    using JudgeSystem.Services.Data;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using JudgeSystem.Services;
    using System.IO;

    [Authorize]
	public class ResourceController : BaseController
	{
		private readonly IResourceService resourceService;
        private readonly IAzureStorageService azureStorageService;

        public ResourceController(
            IResourceService resourceService,
            IAzureStorageService azureStorageService)
		{
			this.resourceService = resourceService;
            this.azureStorageService = azureStorageService;
        }

		public async Task<IActionResult> Download(int id)
		{
			Resource resource = await resourceService.GetById(id);
			var mimeType = "application/octet-stream"; 

			using(var stream = new MemoryStream())
            {
                await azureStorageService.Download(resource.FilePath, stream);
		    	return File(stream.ToArray(), mimeType, resource.Name + System.IO.Path.GetExtension(resource.FilePath)); 
            }
		}
	}
}
