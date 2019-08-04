using System.Globalization;

using JudgeSystem.Common;
using JudgeSystem.Services.Mapping;

using AutoMapper;

namespace JudgeSystem.Web.ViewModels.Contest
{
    public class PreviousContestViewModel : IMapFrom<Data.Models.Contest>, IHaveCustomMappings
	{
		public string Name { get; set; }

		public string LessonId { get; set; }

		public string EndTime { get; set; }

		public void CreateMappings(IProfileExpression configuration)
		{
			configuration.CreateMap<Data.Models.Contest, PreviousContestViewModel>()
				.ForMember(c => c.EndTime, y => y.MapFrom(s => s.EndTime.ToString(GlobalConstants.StandardDateFormat, CultureInfo.InvariantCulture)));
		}
	}
}
