namespace JudgeSystem.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using JudgeSystem.Common;
    using JudgeSystem.Data.Common.Repositories;
    using JudgeSystem.Data.Models;
    using JudgeSystem.Services.Mapping;
    using JudgeSystem.Web.Infrastructure.Exceptions;
    using JudgeSystem.Web.ViewModels.Resource;
    using JudgeSystem.Web.InputModels.Resource;

    using Microsoft.EntityFrameworkCore;

    public class ResourceService : IResourceService
    {
        private readonly IRepository<Resource> repository;

        public ResourceService(IRepository<Resource> repository)
        {
            this.repository = repository;
        }

        public async Task CreateResource(ResourceInputModel model, string filePath)
        {
            Resource resource = new Resource
            {
                Name = model.Name,
                FilePath = filePath,
                LessonId = model.LessonId,
            };

            await repository.AddAsync(resource);
            await repository.SaveChangesAsync();
        }

        public async Task<Resource> GetById(int id)
        {
            return await this.repository.All().FirstOrDefaultAsync(r => r.Id == id);
        }

        public IEnumerable<ResourceViewModel> LessonResources(int lessonId)
        {
            return repository.All()
                .Where(r => r.LessonId == lessonId)
                .To<ResourceViewModel>()
                .ToList();
        }

        public async Task Delete(Resource resource)
        {
            if(!this.repository.All().Any(x => x.Id == resource.Id))
            {
                throw new EntityNotFoundException(nameof(resource));
            }

            repository.Delete(resource);
            await repository.SaveChangesAsync();
        }

        public async Task Update(ResourceEditInputModel model, string filePath = null)
        {
            Resource resource = await GetById(model.Id);

            if (resource == null)
            {
                throw new EntityNotFoundException(nameof(resource));
            }

            if(!string.IsNullOrEmpty(filePath))
            {
                resource.FilePath = filePath;
            }

            resource.Name = model.Name;
            repository.Update(resource);

            await repository.SaveChangesAsync();
        }

        private string AddExtensionToFileName(string name, string fileName)
        {
            return name + "." + GetFileExtension(fileName);
        }
    }
}
