namespace JudgeSystem.Web.InputModels.Student
{
    using JudgeSystem.Services.Mapping;
    using System.ComponentModel.DataAnnotations;
	using JudgeSystem.Data.Models;

	public class StudentCreateInputModel : IMapTo<Student>
	{
		[Required]
		[MinLength(5), MaxLength(100)]
		public int FullName { get; set; }

		[EmailAddress]
		[Required]
		public string Email { get; set; }

		public int NumberInCalss { get; set; }

		public int SchoolClassId { get; set; }
	}
}
