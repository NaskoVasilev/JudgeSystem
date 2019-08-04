using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.Dtos.Course
{
	public class ContestCourseDto : IMapFrom<Data.Models.Course>
	{
		public int Id { get; set; }

		public string Name { get; set; }
	}
}
