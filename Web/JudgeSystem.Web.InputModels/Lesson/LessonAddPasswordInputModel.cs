using System.ComponentModel.DataAnnotations;

using JudgeSystem.Common;

namespace JudgeSystem.Web.InputModels.Lesson
{
	public class LessonAddPasswordInputModel
	{
		public int Id { get; set; }

		[Required]
        [StringLength(ModelConstants.LessonPasswordMaxLength, MinimumLength = ModelConstants.LessonPasswordMinLength)]
        [DataType(DataType.Password)]
        [Display(Name = ModelConstants.LessonPasswordDisplayName)]
		public string LessonPassword { get; set; }
	}
}
