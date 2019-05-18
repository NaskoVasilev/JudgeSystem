namespace JudgeSystem.Web.ViewModels.Course
{
	using JudgeSystem.Services.Mapping;
	using System.ComponentModel.DataAnnotations;
	using JudgeSystem.Data.Models;

	public class CourseInputModel : IMapTo<Course>
	{
		[Required]
		[MinLength(3)]
		public string Name { get; set; }
	}
}
