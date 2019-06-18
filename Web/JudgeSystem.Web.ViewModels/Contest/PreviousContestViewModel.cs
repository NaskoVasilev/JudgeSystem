namespace JudgeSystem.Web.ViewModels.Contest
{
	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Data.Models;

	public class PreviousContestViewModel : IMapFrom<Contest>
	{
		public string Name { get; set; }

		public string LessonId { get; set; }

		public string EndTime { get; set; }
	}
}
