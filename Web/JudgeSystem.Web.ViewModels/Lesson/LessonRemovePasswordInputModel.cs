using System.ComponentModel.DataAnnotations;

namespace JudgeSystem.Web.ViewModels.Lesson
{
	public class LessonRemovePasswordInputModel
	{
		public int Id { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string OldPassword { get; set; }
	}
}
