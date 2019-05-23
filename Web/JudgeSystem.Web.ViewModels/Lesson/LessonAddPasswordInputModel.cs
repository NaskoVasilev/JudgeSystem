namespace JudgeSystem.Web.ViewModels.Lesson
{
	using System.ComponentModel.DataAnnotations;

	using Common;

	public class LessonAddPasswordInputModel
	{
		public int Id { get; set; }

		[Required]
		[MinLength(GlobalConstants.PasswordMinLength)]
		[DataType(DataType.Password)]
		public string LessonPassword { get; set; }
	}
}
