namespace JudgeSystem.Web.ViewModels.Contest
{
	using System;

    using JudgeSystem.Services.Mapping;
    using JudgeSystem.Data.Models;
    using System.ComponentModel.DataAnnotations.Schema;
    using AutoMapper;
    using JudgeSystem.Common;
    using System.Globalization;

    public class ContestViewModel : IMapFrom<Contest>
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public DateTime StartTime { get; set; }

		public DateTime EndTime { get; set; }

		[IgnoreMap]
		public string FormatedStartTime => this.StartTime.ToString(GlobalConstants.StandardDateFormat, CultureInfo.InvariantCulture);

		[IgnoreMap]
		public string FormatedEndTime => this.EndTime.ToString(GlobalConstants.StandardDateFormat, CultureInfo.InvariantCulture);
	}
}
