namespace JudgeSystem.Web.InputModels.Contest
{
	using System;
	using System.ComponentModel.DataAnnotations;

    using JudgeSystem.Services.Mapping;
    using JudgeSystem.Web.Infrastructure.Attributes.Validation;
	using JudgeSystem.Data.Models;

	public class ContestEditInputModel : IMapFrom<Contest>, IMapTo<Contest>
	{
		private const string StartEndTimeErrorMessage = "End time must br after start time.";

		public int Id { get; set; }

		[AfterDateTimeNow]
		public DateTime StartTime { get; set; }

		[AfterDateTimeNow]
		[AfterDate(nameof(StartTime), StartEndTimeErrorMessage)]
		public DateTime EndTime { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 5)]
		public string Name { get; set; }
	}
}
