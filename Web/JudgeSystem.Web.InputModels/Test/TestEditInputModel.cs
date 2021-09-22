using System.ComponentModel.DataAnnotations;

using JudgeSystem.Common;
using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.InputModels.Test
{
    public class TestEditInputModel : IMapTo<Data.Models.Test>, IMapFrom<Data.Models.Test>
	{
		public int Id { get; set; }

        [MaxLength(ModelConstants.TestInputDataMaxLength)]
        [Display(Name = ModelConstants.TestInputDataDisplayName)]
        public string InputData { get; set; }

        [Required]
        [MaxLength(ModelConstants.TestOutputDataMaxLength)]
        [Display(Name = ModelConstants.TestOutputDataDisplayName)]
        public string OutputData { get; set; }

        [Display(Name = ModelConstants.TestIsTrialTestDisplayName)]
        public bool IsTrialTest { get; set; }

        public string ProblemName { get; set; }

        public int ProblemId { get; set; }

        [Range(ModelConstants.OrderByMinValue, ModelConstants.OrderByMaxValue)]
        public int OrderBy { get; set; }
    }
}
