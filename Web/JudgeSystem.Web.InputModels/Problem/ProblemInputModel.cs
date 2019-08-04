using System.ComponentModel.DataAnnotations;

using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Common;
using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.InputModels.Problem
{
	public class ProblemInputModel : IMapTo<Data.Models.Problem>
	{
        public ProblemInputModel()
        {
            this.AllowedMemoryInMegaBytes = GlobalConstants.DefaultAllowedMemoryInMegaBytes;
            this.AllowedTimeInMilliseconds = GlobalConstants.DefaultAllowedTimeInMilliseconds;
            this.MaxPoints = GlobalConstants.DefaultMaxPoints;
        }

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

        public int LessonId { get; set; }

        [Display(Name = ModelConstants.ProblemSubmissionTypeDisplayName)]
        public SubmissionType SubmissionType { get; set; }
    }
}
