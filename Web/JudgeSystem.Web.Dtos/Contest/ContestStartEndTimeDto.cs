namespace JudgeSystem.Web.Dtos.Contest
{
	using System;

	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Data.Models;

	public class ContestStartEndTimeDto : IMapFrom<Contest>
	{
		public DateTime  StartTime { get; set; }

		public DateTime EndTime { get; set; }
	}
}
