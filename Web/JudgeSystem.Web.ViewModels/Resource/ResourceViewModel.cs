namespace JudgeSystem.Web.ViewModels.Resource
{
	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Data.Models;

	public class ResourceViewModel : IMapFrom<Resource>
	{
		public int Id { get; set; }

		public string Name { get; set; }
	}
}
