using System;
using System.ComponentModel.DataAnnotations;

using JudgeSystem.Common;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.Infrastructure.Attributes.Validation;

namespace JudgeSystem.Web.InputModels.Contest
{
	public class ContestCreateInputModel : IMapTo<Data.Models.Contest>
	{
		public const string StartEndTimeErrorMessage = "End time must be after start time.";

		[AfterDateTimeNow]
        [Display(Name = ModelConstants.ContestStartTimeDisplayName)]
		public DateTime StartTime { get; set; }

		[AfterDateTimeNow]
		[AfterDate(nameof(StartTime), StartEndTimeErrorMessage)]
        [Display(Name = ModelConstants.ContestEndTimeDisplayName)]
        public DateTime EndTime { get; set; }

        [Display(Name = ModelConstants.ContestLessonIdDisplayName)]
        public int LessonId { get; set; }

		[Required]
		[StringLength(ModelConstants.ContestNameMaxLength, MinimumLength = ModelConstants.ContestNameMinLength)]
		public string Name { get; set; }
	}
}
