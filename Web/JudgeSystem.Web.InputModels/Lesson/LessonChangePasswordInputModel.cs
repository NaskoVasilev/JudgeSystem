namespace JudgeSystem.Web.InputModels.Lesson
{
	using System.ComponentModel.DataAnnotations;

	using Common;

	public class LessonChangePasswordInputModel
	{
		public int Id { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string OldPassword { get; set; }

		//TODO: Use some regex for the password for more security
		[Required]
		[MinLength(GlobalConstants.PasswordMinLength)]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }
	}
}
