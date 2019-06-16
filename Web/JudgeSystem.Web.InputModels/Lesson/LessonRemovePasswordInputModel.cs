namespace JudgeSystem.Web.InputModels.Lesson
{
	using System.ComponentModel.DataAnnotations;

	public class LessonRemovePasswordInputModel
	{
		public int Id { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string OldPassword { get; set; }
	}
}
