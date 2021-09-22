using System.ComponentModel.DataAnnotations;

using JudgeSystem.Common;
using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.InputModels.Course
{
	public class CourseEditModel : IMapTo<Data.Models.Course>, IMapFrom<Data.Models.Course>
	{
		public int Id { get; set; }

		[Required]
		[StringLength(ModelConstants.CourseNameMaxLength, MinimumLength = ModelConstants.CourseNameMinLength)]
		public string Name { get; set; }

        [Range(ModelConstants.OrderByMinValue, ModelConstants.OrderByMaxValue)]
        public int OrderBy { get; set; }
    }
}
