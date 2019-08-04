using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.Dtos.Lesson
{
	public class ContestLessonDto : IMapFrom<Data.Models.Lesson>
	{
		public string Name { get; set; }

		public int Id { get; set; }
	}
}
