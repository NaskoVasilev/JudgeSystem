namespace JudgeSystem.Services.Data
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	using JudgeSystem.Data.Models;
	using JudgeSystem.Web.ViewModels.Resource;
	using JudgeSystem.Web.InputModels.Resource;

	public interface IResourceService
	{
		Task<Resource> GetById(int id);

		Task CreateResource(ResourceInputModel model, string filePath);

		IEnumerable<ResourceViewModel> LessonResources(int lessonId);

		Task Update(ResourceEditInputModel model, string filePath = null);


		Task Delete(Resource resource);
	}
}
