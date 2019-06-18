namespace JudgeSystem.Web.ViewModels.Lesson
{
	using System.Collections.Generic;

	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Data.Models;

	using Web.ViewModels.Problem;
	using Web.ViewModels.Resource;

	public class LessonViewModel : IMapFrom<Lesson>
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public int CourseId { get; set; }

		public string LessonPassword { get; set; }

		public int? ContestId { get; set; }

		public bool IsLocked => LessonPassword != null;

		public ICollection<ResourceViewModel> Resources { get; set; }

		public ICollection<ProblemViewModel> Problems { get; set; }
	}
}
