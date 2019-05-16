using System.Collections.Generic;
using JudgeSystem.Data.Common.Models;

namespace JudgeSystem.Data.Models
{
	public class Lesson : BaseDeletableModel<int>
	{
		public Lesson()
		{
			this.Resources = new List<Resource>();
		}

		public string Name { get; set; }

		public int CourseId { get; set; }
		public Course Course { get; set; }

		public string LessonPassword { get; set; }

		public bool IsLocked => LessonPassword != null;

		public bool IsExam { get; set; }

		public ICollection<Resource> Resources { get; set; }
	}
}
