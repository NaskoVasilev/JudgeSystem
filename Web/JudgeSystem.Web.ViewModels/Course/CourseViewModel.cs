namespace JudgeSystem.Web.ViewModels.Course
{
	using Services.Mapping;
	using Data.Models;

	public class CourseViewModel : IMapFrom<Course>
	{
		public int Id { get; set; }

		public string Name { get; set; }
	}
}
