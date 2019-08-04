using System.Collections.Generic;

using JudgeSystem.Services.Mapping;

using JudgeSystem.Web.ViewModels.Problem;
using JudgeSystem.Web.ViewModels.Resource;

namespace JudgeSystem.Web.ViewModels.Lesson
{
    public class LessonViewModel : IMapFrom<Data.Models.Lesson>
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public int CourseId { get; set; }

		public string LessonPassword { get; set; }

		public int? ContestId { get; set; }

        public int PracticeId { get; set; }

        public bool IsLocked => LessonPassword != null;

		public ICollection<ResourceViewModel> Resources { get; set; }

		public ICollection<ProblemViewModel> Problems { get; set; }
	}
}
