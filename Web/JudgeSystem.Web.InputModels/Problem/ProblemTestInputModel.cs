using System.ComponentModel.DataAnnotations;

using JudgeSystem.Common;
using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.InputModels.Problem
{
    public class ProblemTestInputModel : IMapTo<Data.Models.Test>
    {
        [MaxLength(ModelConstants.TestInputDataMaxLength)]
        public string InputData { get; set; }

        [Required]
        [MaxLength(ModelConstants.TestOutputDataMaxLength)]
        public string OutputData { get; set; }

        [Required]
        public bool IsTrialTest { get; set; }
    }
}
