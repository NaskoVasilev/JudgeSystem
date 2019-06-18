namespace JudgeSystem.Web.ViewModels.Lesson
{
	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Data.Models;

	public class LessonContestViewModel : IMapFrom<Contest>
	{
		public int Id { get; set; }

		public string Name { get; set; }
	}
}
