using JudgeSystem.Data.Models;
using JudgeSystem.Web.ViewModels.Resource;
using System.Threading.Tasks;

namespace JudgeSystem.Services.Data
{
	public interface IResourceService
	{
		Resource CreateResource(string filePath, string fileName);

		Task<Resource> GetById(int id);

		Task CreateResource(ResourceInputModel model, string fileName);
	}
}
