using System;
using System.Globalization;

using JudgeSystem.Services.Mapping;
using JudgeSystem.Common;

using AutoMapper;

namespace JudgeSystem.Web.ViewModels.Contest
{
    public class ContestViewModel : IMapFrom<Data.Models.Contest>
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public DateTime StartTime { get; set; }

		public DateTime EndTime { get; set; }

        [IgnoreMap]
		public string FormatedStartTime => StartTime.ToString(GlobalConstants.StandardDateFormat, CultureInfo.InvariantCulture);

		[IgnoreMap]
		public string FormatedEndTime => EndTime.ToString(GlobalConstants.StandardDateFormat, CultureInfo.InvariantCulture);
	}
}
