namespace JudgeSystem.Web.InputModels.Course
{
	using JudgeSystem.Data.Models;
	using Services.Mapping;
	using System.ComponentModel.DataAnnotations;

	public class CourseEditModel : IMapTo<Course>, IMapFrom<Course>
	{
		public int Id { get; set; }

		[Required]
		[MinLength(3)]
		public string Name { get; set; }
	}
}
