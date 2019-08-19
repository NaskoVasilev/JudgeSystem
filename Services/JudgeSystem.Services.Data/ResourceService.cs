using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.ViewModels.Resource;
using JudgeSystem.Web.InputModels.Resource;
using JudgeSystem.Web.Dtos.Resource;
using JudgeSystem.Common;

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

        public async Task CreateResource(ResourceInputModel model, string filePath)
        {
            var resource = new Resource
            {
                Name = model.Name,
                FilePath = filePath,
                LessonId = model.LessonId,
            };

            await repository.AddAsync(resource);
        }

        public async Task<TDestination> GetById<TDestination>(int id)
        {
            TDestination resource = await repository.All().Where(x => x.Id == id).To<TDestination>().FirstOrDefaultAsync();
            Validator.ThrowEntityNotFoundExceptionIfEntityIsNull(resource, nameof(Resource));
            return resource;
        }

        public IEnumerable<ResourceViewModel> LessonResources(int lessonId)
        {
            var resources = repository.All()
                .Where(r => r.LessonId == lessonId)
                .To<ResourceViewModel>()
                .ToList();
            return resources;
        }

        public async Task<ResourceDto> Delete(int id)
        {
            Resource resource = await repository.FindAsync(id);
            await repository.DeleteAsync(resource);
            return resource.To<ResourceDto>();
        }

        public async Task Update(ResourceEditInputModel model, string filePath = null)
        {
            Resource resource = await repository.FindAsync(model.Id);
            if(!string.IsNullOrEmpty(filePath))
            {
                resource.FilePath = filePath;
            }

            resource.Name = model.Name;
            await repository.UpdateAsync(resource);
        }
    }
}
