namespace JudgeSystem.Web.ViewModels.Contest
{
	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Data.Models;

	public class ContestBreifInfoViewModel : IMapFrom<Contest>
	{
		public string Name { get; set; }

		public int Id { get; set; }
	}
}
