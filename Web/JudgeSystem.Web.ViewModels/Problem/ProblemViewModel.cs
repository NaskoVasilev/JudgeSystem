namespace JudgeSystem.Web.ViewModels.Problem
{
	using Services.Mapping;
	using Data.Models;

	public class ProblemViewModel : IMapFrom<Problem>
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public bool IsExtraTask { get; set; }

		public int MaxPoints { get; set; }
	}
}
