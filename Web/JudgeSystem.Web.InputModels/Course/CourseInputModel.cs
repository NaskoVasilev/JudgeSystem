namespace JudgeSystem.Web.InputModels.Course
{
	using System.ComponentModel.DataAnnotations;

	using Services.Mapping;
	using Data.Models;

	public class CourseInputModel : IMapTo<Course>
	{
		[Required]
		[MinLength(3)]
		public string Name { get; set; }
	}
}
