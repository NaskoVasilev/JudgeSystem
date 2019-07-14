namespace JudgeSystem.Web.ViewModels.Contest
{
	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Data.Models;
	using AutoMapper;
    using JudgeSystem.Common;
    using System.Globalization;

    public class PreviousContestViewModel : IMapFrom<Contest>, IHaveCustomMappings
	{
		public string Name { get; set; }

		public string LessonId { get; set; }

		public string EndTime { get; set; }

		public void CreateMappings(IProfileExpression configuration)
		{
			configuration.CreateMap<Contest, PreviousContestViewModel>()
				.ForMember(c => c.EndTime, y => y.MapFrom(s => s.EndTime.ToString(GlobalConstants.StandardDateFormat, CultureInfo.InvariantCulture)));
		}
	}
}
