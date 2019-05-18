using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using JudgeSystem.Data.Common.Models;
using JudgeSystem.Data.Models.Enums;

namespace JudgeSystem.Data.Models
{
	public class Lesson : BaseDeletableModel<int>
	{
		public Lesson()
		{
			this.Resources = new List<Resource>();
			this.Problems = new List<Problem>();
		}

		public string Name { get; set; }

		public int CourseId { get; set; }
		public Course Course { get; set; }

		public string LessonPassword { get; set; }

		[NotMapped]
		public bool IsLocked => LessonPassword != null;

		public LessonType Type { get; set; }

		public ICollection<Resource> Resources { get; set; }

		public ICollection<Problem> Problems { get; set; }
	}
}
