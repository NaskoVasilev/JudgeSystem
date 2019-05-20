using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Services.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Threading.Tasks;

namespace JudgeSystem.Web.Controllers
{
	public class ResourceController : BaseController
	{
		private readonly IResourceService resourceService;

		public ResourceController(IResourceService resourceService)
		{
			this.resourceService = resourceService;
		}

		public async Task<IActionResult> Download(int id)
		{
			Resource resource = await resourceService.GetById(id);
			// Construct the path to the physical files folder
			string filePath = GlobalConstants.FileStorePath;

			IFileProvider provider = new PhysicalFileProvider(filePath); // Initialize the Provider
			IFileInfo fileInfo = provider.GetFileInfo(resource.Link); // Extract the FileInfo

			var readStream = fileInfo.CreateReadStream(); // Extact the Stream
			var mimeType = "application/octet-stream"; // Set a mimeType

			return File(readStream, mimeType, resource.Name); // Return FileResult
		}
	}
}
