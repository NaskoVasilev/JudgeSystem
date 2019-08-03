namespace JudgeSystem.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using JudgeSystem.Data.Common.Repositories;
    using JudgeSystem.Data.Models;
    using JudgeSystem.Services.Mapping;
    using JudgeSystem.Web.Infrastructure.Exceptions;
    using JudgeSystem.Web.ViewModels.Resource;
    using JudgeSystem.Web.InputModels.Resource;

    using Microsoft.EntityFrameworkCore;
    using JudgeSystem.Web.Dtos.Resource;

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

        public async Task<TDestination> GetById<TDestination>(int id)
        {
            var resource = await this.repository.All().Where(x => x.Id == id).To<TDestination>().FirstOrDefaultAsync();
            if(resource == null)
            {
                throw new EntityNotFoundException(nameof(resource));
            }
            return resource;
        }

        public IEnumerable<ResourceViewModel> LessonResources(int lessonId)
        {
            return repository.All()
                .Where(r => r.LessonId == lessonId)
                .To<ResourceViewModel>()
                .ToList();
        }

        public async Task<ResourceDto> Delete(int id)
        {
            Resource resource = await repository.All().FirstOrDefaultAsync(x => x.Id == id);
            if(resource == null)
            {
                throw new EntityNotFoundException(nameof(resource));
            }

            repository.Delete(resource);
            await repository.SaveChangesAsync();
            return resource.To<ResourceDto>();
        }

        public async Task Update(ResourceEditInputModel model, string filePath = null)
        {
            Resource resource = await repository.All().FirstOrDefaultAsync(x => x.Id == model.Id);

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
    }
}
