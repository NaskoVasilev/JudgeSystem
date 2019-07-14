namespace JudgeSystem.Web.ViewModels.Lesson
{
	using Services.Mapping;
	using Data.Models;
	using Data.Models.Enums;

	using AutoMapper;
	using System.Collections.Generic;
	using System.Linq;
	using System;

	public class LessonLinkViewModel : IMapFrom<Lesson>, IHaveCustomMappings
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public int ProblemsCount { get; set; }

		public int CourseId { get; set; }

		public LessonType Type { get; set; }

		public List<LessonContestViewModel> Contests { get; set; }

		public void CreateMappings(IProfileExpression configuration)
		{
			configuration.CreateMap<Lesson, LessonLinkViewModel>()
				.ForMember(x => x.ProblemsCount, y => y.MapFrom(s => s.Problems.Count))
				.ForMember(x => x.Contests, y => y.MapFrom(s => s.Contests
				.Where(c => c.StartTime < DateTime.Now && c.EndTime > DateTime.Now)));
		}
	}
}
