using System;

using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.Dtos.Contest
{
    public class ContestStartEndTimeDto : IMapFrom<Data.Models.Contest>
	{
        public string Name { get; set; }

        public DateTime  StartTime { get; set; }

		public DateTime EndTime { get; set; }
	}
}
