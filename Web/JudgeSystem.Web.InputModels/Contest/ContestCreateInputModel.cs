namespace JudgeSystem.Web.InputModels.Contest
{
	using System;

	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Data.Models;
	using JudgeSystem.Web.Infrastructure.Attributes.Validation;
	using System.ComponentModel.DataAnnotations;

	public class ContestCreateInputModel : IMapTo<Contest>
	{
		private const string StartEndTimeErrorMessage = "End time must br after start time.";

		[AfterDateTimeNow]
		public DateTime StartTime { get; set; }

		[AfterDateTimeNow]
		[AfterDate(nameof(StartTime), StartEndTimeErrorMessage)]
		public DateTime EndTime { get; set; }

		public int LessonId { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 5)]
		public string Name { get; set; }
	}
}
