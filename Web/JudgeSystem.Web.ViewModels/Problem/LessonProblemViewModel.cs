namespace JudgeSystem.Web.ViewModels.Problem
{
	using Services.Mapping;
	using Data.Models;

	public class LessonProblemViewModel : IMapFrom<Problem>
	{
		public int Id { get; set; }

		public string Name { get; set; }
	}
}
