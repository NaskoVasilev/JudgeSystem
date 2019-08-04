using System.ComponentModel.DataAnnotations;

using JudgeSystem.Common;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.InputModels.Problem
{
	public class ProblemEditInputModel : IMapTo<Data.Models.Problem>, IMapFrom<Data.Models.Problem>
	{
		public int Id { get; set; }

		[Required]
        [StringLength(ModelConstants.ProblemNameMaxLength, MinimumLength = ModelConstants.ProblemNameMinLength)]
        public string Name { get; set; }

        [Display(Name = ModelConstants.ProblemIsExtraTaskDisplayName)]
		public bool IsExtraTask { get; set; }
		
		[Range(ModelConstants.ProblemMinPoints, ModelConstants.ProblemMaxPoints)]
        [Display(Name = ModelConstants.ProblemMaxPointsDisplayName)]
        public int MaxPoints { get; set; }


        [Range(GlobalConstants.MinAllowedTimeInMilliseconds, GlobalConstants.MaxAllowedTimeInMilliseconds)]
        [Display(Name = ModelConstants.ProblemAllowedTimeInMillisecondsDisplayName)]
        public int AllowedTimeInMilliseconds { get; set; }

        [Range(GlobalConstants.MinAllowedMemoryInMegaBytes, GlobalConstants.MaxAllowedMemoryInMegaBytes)]
        [Display(Name = ModelConstants.ProblemAllowedMemoryInMegaBytesDisplayName)]
        public double AllowedMemoryInMegaBytes { get; set; }

        [Display(Name = ModelConstants.ProblemSubmissionTypeDisplayName)]
        public SubmissionType SubmissionType { get; set; }
    }
}
