using JudgeSystem.Common;
using System.ComponentModel.DataAnnotations;

namespace JudgeSystem.Web.InputModels.Lesson
{
	public class LessonPasswordInputModel
	{
		public int Id { get; set; }

        [Required]
        [StringLength(ModelConstants.LessonPasswordMaxLength, MinimumLength = ModelConstants.LessonPasswordMinLength)]
        [DataType(DataType.Password)]
        [Display(Name = ModelConstants.LessonPasswordDisplayName)]
        public string LessonPassword { get; set; }

        public int? PracticeId { get; set; }

        public int? ContestId { get; set; }
    }
}
