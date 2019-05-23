namespace JudgeSystem.Web.ViewModels.Course
{
	using System.ComponentModel.DataAnnotations;

	using Services.Mapping;
	using Data.Models;

	public class CourseEditModel : IMapTo<Course>
	{
		public int Id { get; set; }

		[Required]
		[MinLength(3)]
		public string Name { get; set; }
	}
}
