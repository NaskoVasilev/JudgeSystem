using System.ComponentModel.DataAnnotations;

using JudgeSystem.Common;

namespace JudgeSystem.Web.InputModels.Lesson
{
	public class LessonRemovePasswordInputModel
	{
		public int Id { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = ModelConstants.LessonOldPasswordDisplayName)]
        public string OldPassword { get; set; }
	}
}
