using JudgeSystem.Services.Mapping;
using JudgeSystem.Data.Models;

namespace JudgeSystem.Web.ViewModels.Resource
{
	public class ResourceViewModel : IMapFrom<Data.Models.Resource>
	{
		public int Id { get; set; }

		public string Name { get; set; }
	}
}
