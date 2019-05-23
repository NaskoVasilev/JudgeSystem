namespace JudgeSystem.Web.ViewModels.Resource
{
	using Services.Mapping;
	using Data.Models;

	public class ResourceViewModel : IMapFrom<Resource>
	{
		public int Id { get; set; }

		public string Name { get; set; }
	}
}
