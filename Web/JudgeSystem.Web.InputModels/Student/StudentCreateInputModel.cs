namespace JudgeSystem.Web.InputModels.Student
{
    using System.ComponentModel.DataAnnotations;

    using JudgeSystem.Services.Mapping;
	using JudgeSystem.Data.Models;

	public class StudentCreateInputModel : IMapTo<Student>
	{
		[Required]
		[MinLength(5), MaxLength(100)]
		public string FullName { get; set; }

		[EmailAddress]
		[Required]
		public string Email { get; set; }

		public int NumberInCalss { get; set; }

		public int SchoolClassId { get; set; }
	}
}
