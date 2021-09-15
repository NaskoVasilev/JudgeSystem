using System.ComponentModel.DataAnnotations;

using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Common;

namespace JudgeSystem.Web.InputModels.Lesson
{
    public class LessonInputModel : IMapTo<Data.Models.Lesson>
	{
		[Required]
        [StringLength(ModelConstants.LessonNameMaxLength, MinimumLength = ModelConstants.LessonNameMinLength)]
        public string Name { get; set; }

		public int CourseId { get; set; }

		[Display(Name = ModelConstants.LessonPasswordDisplayName)]
		[DataType(DataType.Password)]
        [StringLength(ModelConstants.LessonPasswordMaxLength, MinimumLength = ModelConstants.LessonPasswordMinLength)]
        public string LessonPassword { get; set; }

		[Required]
		public LessonType Type { get; set; }

        [Range(ModelConstants.OrderByMinValue, ModelConstants.OrderByMaxValue)]
        public int OrderBy { get; set; }

    }
}
