using System;
using System.ComponentModel.DataAnnotations;

using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.Infrastructure.Attributes.Validation;
using JudgeSystem.Common;

namespace JudgeSystem.Web.InputModels.Contest
{
    public class ContestEditInputModel : IMapFrom<Data.Models.Contest>, IMapTo<Data.Models.Contest>
	{
		private const string StartEndTimeErrorMessage = "End time must br after start time.";

		public int Id { get; set; }

		[AfterDateTimeNow]
		public DateTime StartTime { get; set; }

		[AfterDateTimeNow]
		[AfterDate(nameof(StartTime), StartEndTimeErrorMessage)]
		public DateTime EndTime { get; set; }

		[Required]
		[StringLength(ModelConstants.ContestNameMaxLength, MinimumLength = ModelConstants.ContestNameMinLength)]
		public string Name { get; set; }
	}
}
