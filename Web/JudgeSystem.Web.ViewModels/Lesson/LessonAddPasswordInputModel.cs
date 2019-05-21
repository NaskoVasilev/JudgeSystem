using JudgeSystem.Common;
using System.ComponentModel.DataAnnotations;

namespace JudgeSystem.Web.ViewModels.Lesson
{
	public class LessonAddPasswordInputModel
	{
		public int Id { get; set; }

		[Required]
		[MinLength(GlobalConstants.PasswordMinLength)]
		[DataType(DataType.Password)]
		public string LessonPassword { get; set; }
	}
}
