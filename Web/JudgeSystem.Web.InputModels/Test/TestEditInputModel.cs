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
	}
}
