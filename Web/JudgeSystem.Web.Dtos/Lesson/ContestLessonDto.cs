namespace JudgeSystem.Web.Dtos.Lesson
{
	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Data.Models;

	public class ContestLessonDto : IMapFrom<Lesson>
	{
		public string Name { get; set; }

		public int Id { get; set; }
	}
}
