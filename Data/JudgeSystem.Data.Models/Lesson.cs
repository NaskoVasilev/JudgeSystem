namespace JudgeSystem.Data.Models
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	using Data.Common.Models;
	using Data.Models.Enums;

	public class Lesson : BaseDeletableModel<int>
	{
		public Lesson()
		{
			this.Resources = new List<Resource>();
			this.Problems = new List<Problem>();
		}

		[Required]
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
