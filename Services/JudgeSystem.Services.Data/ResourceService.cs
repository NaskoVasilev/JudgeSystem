using System;
using System.Threading.Tasks;
using JudgeSystem.Common;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Web.ViewModels.Resource;
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

		public async Task CreateResource(ResourceInputModel model, string fileName)
		{
			string nameWithExtension = AddExtensionToFileName(model.Name, fileName);
			Resource resource = new Resource
			{
				Name = nameWithExtension,
				Link = fileName,
				LessonId = model.LessonId,
				ResourceType = model.ResourceType
			};

			await repository.AddAsync(resource);
			await repository.SaveChangesAsync();
		}

		public async Task<Resource> GetById(int id)
		{
			return await this.repository.All().FirstOrDefaultAsync(r => r.Id == id);
		}

		private ResourceType GetResourseType(string filePath)
		{
			ResourceType resourceType = ResourceType.ProblemsDescription;

			if (filePath.EndsWith(GlobalConstants.WordFileExtension))
			{
				resourceType = ResourceType.ProblemsDescription;
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

		private string AddExtensionToFileName(string name, string fileName)
		{
			string[] fileNameParts = fileName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
			if(fileNameParts.Length > 0)
			{
				string fileExtension = fileNameParts[fileNameParts.Length - 1];
				return name + '.' + fileExtension;
			}

			return name + ".txt";
		}
	}
}
