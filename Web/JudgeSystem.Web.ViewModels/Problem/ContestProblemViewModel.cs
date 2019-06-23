namespace JudgeSystem.Web.ViewModels.Problem
{
	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Data.Models;

	public class ContestProblemViewModel : IMapFrom<Problem>
	{
		public int Id { get; set; }

		public string Name { get; set; }
	}
}
