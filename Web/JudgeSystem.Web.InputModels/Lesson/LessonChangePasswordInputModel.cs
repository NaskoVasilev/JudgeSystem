using System.ComponentModel.DataAnnotations;

using JudgeSystem.Common;

namespace JudgeSystem.Web.InputModels.Lesson
{
	public class LessonChangePasswordInputModel
	{
		public int Id { get; set; }

		[Required]
		[DataType(DataType.Password)]
        [Display(Name = ModelConstants.LessonOldPasswordDisplayName)]
		public string OldPassword { get; set; }

		[Required]
        [StringLength(ModelConstants.LessonPasswordMaxLength, MinimumLength = ModelConstants.LessonPasswordMinLength)]
        [DataType(DataType.Password)]
        [Display(Name = ModelConstants.LessonPasswordDisplayName)]
		public string NewPassword { get; set; }
	}
}
