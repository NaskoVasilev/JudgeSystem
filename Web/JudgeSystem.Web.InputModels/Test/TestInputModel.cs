using System.ComponentModel.DataAnnotations;

using JudgeSystem.Common;
using JudgeSystem.Services.Mapping;

using AutoMapper;


namespace JudgeSystem.Web.InputModels.Test
{
    public class TestInputModel : IMapTo<Data.Models.Test>
	{
		public int Id { get; set; }
		
		public int ProblemId { get; set; }

		[MaxLength(ModelConstants.TestInputDataMaxLength)]
        [Display(Name = ModelConstants.TestInputDataDisplayName)]
        public string InputData { get; set; }

		[Required]
        [MaxLength(ModelConstants.TestOutputDataMaxLength)]
        [Display(Name = ModelConstants.TestOutputDataDisplayName)]
        public string OutputData { get; set; }

        [Display(Name = ModelConstants.TestIsTrialTestDisplayName)]
        public bool IsTrialTest { get; set; }

        [IgnoreMap]
        public int LessonId { get; set; }

        public string ProblemName { get; set; }

        [Range(ModelConstants.OrderByMinValue, ModelConstants.OrderByMaxValue)]
        public int OrderBy { get; set; }
    }
}
