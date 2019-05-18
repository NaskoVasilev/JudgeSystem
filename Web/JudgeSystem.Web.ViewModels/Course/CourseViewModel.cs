namespace JudgeSystem.Web.ViewModels.Course
{
	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Data.Models;

	public class CourseViewModel : IMapFrom<Course>
	{
		public int Id { get; set; }

		public string Name { get; set; }
	}
}
