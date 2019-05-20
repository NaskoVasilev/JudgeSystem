using JudgeSystem.Web.ViewModels.Problem;
using JudgeSystem.Web.ViewModels.Resource;
using System.Collections.Generic;

namespace JudgeSystem.Web.ViewModels.Lesson
{
	public class LessonViewModel
	{
		public string Name { get; set; }

		public int CourseId { get; set; }

		public string LessonPassword { get; set; }

		public bool IsLocked => LessonPassword != null;

		public ICollection<ResourceViewModel> Resources { get; set; }

		public ICollection<ProblemViewModel> Problems { get; set; }
	}
}
