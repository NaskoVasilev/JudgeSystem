using System.ComponentModel.DataAnnotations;

using JudgeSystem.Common;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.InputModels.Lesson
{
	public class LessonEditInputModel : IMapTo<Data.Models.Lesson>, IMapFrom<Data.Models.Lesson>
	{
		public int Id { get; set; }

		[Required]
        [StringLength(ModelConstants.LessonNameMaxLength, MinimumLength = ModelConstants.LessonNameMinLength)]
        public string Name { get; set; }

		public LessonType Type { get; set; }

		public bool IsLocked { get; set; }

        [Range(ModelConstants.OrderByMinValue, ModelConstants.OrderByMaxValue)]
        public int OrderBy { get; set; }
    }
}
