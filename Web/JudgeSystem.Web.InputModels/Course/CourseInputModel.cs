using System.ComponentModel.DataAnnotations;

using JudgeSystem.Services.Mapping;
using JudgeSystem.Common;

namespace JudgeSystem.Web.InputModels.Course
{
    public class CourseInputModel : IMapTo<Data.Models.Course>
	{
		[Required]
        [StringLength(ModelConstants.CourseNameMaxLength, MinimumLength = ModelConstants.CourseNameMinLength)]
        public string Name { get; set; }

        [Range(ModelConstants.OrderByMinValue, ModelConstants.OrderByMaxValue)]
        public int OrderBy { get; set; }
    }
}
