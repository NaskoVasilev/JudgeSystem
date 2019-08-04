using System;
using System.ComponentModel.DataAnnotations;

using JudgeSystem.Common;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.Infrastructure.Attributes.Validation;

namespace JudgeSystem.Web.InputModels.Contest
{
	public class ContestCreateInputModel : IMapTo<Data.Models.Contest>
	{
		private const string StartEndTimeErrorMessage = "End time must be after start time.";

		[AfterDateTimeNow]
		public DateTime StartTime { get; set; }

		[AfterDateTimeNow]
		[AfterDate(nameof(StartTime), StartEndTimeErrorMessage)]
		public DateTime EndTime { get; set; }

		public int LessonId { get; set; }

		[Required]
		[StringLength(ModelConstants.ContestNameMaxLength, MinimumLength = ModelConstants.ContestNameMinLength)]
		public string Name { get; set; }
	}
}
