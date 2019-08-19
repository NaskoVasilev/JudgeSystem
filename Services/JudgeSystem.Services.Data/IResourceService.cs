using System.Collections.Generic;
using System.Threading.Tasks;

using JudgeSystem.Web.ViewModels.Resource;
using JudgeSystem.Web.InputModels.Resource;
using JudgeSystem.Web.Dtos.Resource;

namespace JudgeSystem.Services.Data
{
    public interface IResourceService
	{
		Task<TDestination> GetById<TDestination>(int id);

		Task CreateResource(ResourceInputModel model, string filePath);

		IEnumerable<ResourceViewModel> LessonResources(int lessonId);

		Task Update(ResourceEditInputModel model, string filePath = null);

		Task<ResourceDto> Delete(int id);
	}
}
