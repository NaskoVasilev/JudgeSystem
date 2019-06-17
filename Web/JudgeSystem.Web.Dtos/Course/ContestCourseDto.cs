namespace JudgeSystem.Web.Dtos.Course
{
	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Data.Models;

	public class ContestCourseDto : IMapFrom<Course>
	{
		public int Id { get; set; }

		public string Name { get; set; }
	}
}
