using System.ComponentModel.DataAnnotations;

using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services.Mapping;
using static JudgeSystem.Common.GlobalConstants;
using static JudgeSystem.Common.ModelConstants;

namespace JudgeSystem.Web.InputModels.Problem
{
    public class ProblemEditInputModel : IMapTo<Data.Models.Problem>, IMapFrom<Data.Models.Problem>
	{
		public int Id { get; set; }

		[Required]
        [StringLength(ProblemNameMaxLength, MinimumLength = ProblemNameMinLength)]
        public string Name { get; set; }

        [Display(Name = ProblemIsExtraTaskDisplayName)]
		public bool IsExtraTask { get; set; }
		
		[Range(ProblemMinPoints, ProblemMaxPoints)]
        [Display(Name = ProblemMaxPointsDisplayName)]
        public int MaxPoints { get; set; }


        [Range(MinAllowedTimeInMilliseconds, MaxAllowedTimeInMilliseconds)]
        [Display(Name = ProblemAllowedTimeInMillisecondsDisplayName)]
        public int AllowedTimeInMilliseconds { get; set; }

        [Range(MinAllowedMemoryInMegaBytes, MaxAllowedMemoryInMegaBytes)]
        [Display(Name = ProblemAllowedMemoryInMegaBytesDisplayName)]
        public double AllowedMemoryInMegaBytes { get; set; }

        [Range(ProblemMinTimeIntervalBetweenSubmissionInSeconds, ProblemMaxTimeIntervalBetweenSubmissionInSeconds)]
        [Display(Name = ProblemTimeIntervalBetweenSubmissionInSecondsDisplayName)]
        public int TimeIntervalBetweenSubmissionInSeconds { get; set; }

        [Display(Name = ProblemSubmissionTypeDisplayName)]
        public SubmissionType SubmissionType { get; set; }
    }
}
