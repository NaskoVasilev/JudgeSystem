using System;
using System.ComponentModel.DataAnnotations;

using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.Infrastructure.Attributes.Validation;
using JudgeSystem.Common;

namespace JudgeSystem.Web.InputModels.Contest
{
    public class ContestEditInputModel : IMapFrom<Data.Models.Contest>, IMapTo<Data.Models.Contest>
	{
		public const string StartEndTimeErrorMessage = "End time must br after start time.";

		public int Id { get; set; }

		[AfterDateTimeNow]
        [Display(Name = ModelConstants.ContestStartTimeDisplayName)]
        public DateTime StartTime { get; set; }

		[AfterDateTimeNow]
		[AfterDate(nameof(StartTime), StartEndTimeErrorMessage)]
        [Display(Name = ModelConstants.ContestEndTimeDisplayName)]
        public DateTime EndTime { get; set; }

		[Required]
		[StringLength(ModelConstants.ContestNameMaxLength, MinimumLength = ModelConstants.ContestNameMinLength)]
		public string Name { get; set; }
	}
}
