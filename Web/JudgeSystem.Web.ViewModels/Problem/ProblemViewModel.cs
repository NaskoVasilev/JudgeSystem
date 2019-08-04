using System.ComponentModel.DataAnnotations;

using JudgeSystem.Services.Mapping;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Common;

namespace JudgeSystem.Web.ViewModels.Problem
{
    public class ProblemViewModel : IMapFrom<Data.Models.Problem>
	{
		public int Id { get; set; }

		public string Name { get; set; }

        [Display(Name = ModelConstants.ProblemIsExtraTaskDisplayName)]
        public bool IsExtraTask { get; set; }

        [Display(Name = ModelConstants.ProblemMaxPointsDisplayName)]
        public int MaxPoints { get; set; }

        [Display(Name = ModelConstants.ProblemAllowedTimeInMillisecondsDisplayName)]
        public int AllowedTimeInMilliseconds { get; set; }

        [Display(Name = ModelConstants.ProblemAllowedMemoryInMegaBytesDisplayName)]
        public double AllowedMemoryInMegaBytes { get; set; }

        [Display(Name = ModelConstants.ProblemSubmissionTypeDisplayName)]
        public SubmissionType SubmissionType { get; set; }
    }
}
