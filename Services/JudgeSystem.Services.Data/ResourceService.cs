using System.Threading.Tasks;
using JudgeSystem.Common;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace JudgeSystem.Services.Data
{
	public class ResourceService : IResourceService
	{
		private readonly IRepository<Resource> repository;

		public ResourceService(IRepository<Resource> repository)
		{
			this.repository = repository;
		}

		public Resource CreateResource(string filePath, string fileName)
		{
			ResourceType resourceType = GetResourseType(filePath);
			Resource resource = new Resource()
			{
				Name = fileName,
				ResourceType = resourceType,
				Link = filePath
			};

			return resource;
		}

		public async Task<Resource> GetById(int id)
		{
			return await this.repository.All().FirstOrDefaultAsync(r => r.Id == id);
		}

		private ResourceType GetResourseType(string filePath)
		{
			ResourceType resourceType = ResourceType.PreblemsDescription;

			if (filePath.EndsWith(GlobalConstants.WordFileExtension))
			{
				resourceType = ResourceType.PreblemsDescription;
			}
			else if (filePath.EndsWith(GlobalConstants.PowerPointFileExtension))
			{
				resourceType = ResourceType.Presentation;
			}
			else if (filePath.EndsWith(GlobalConstants.VideoFileExtension))
			{
				resourceType = ResourceType.Video;
			}
			else if (filePath.EndsWith(GlobalConstants.AdministrationArea))
			{
				resourceType = ResourceType.AuthorsSolution;
			}

			return resourceType;
		}
	}
}
