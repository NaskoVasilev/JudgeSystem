namespace JudgeSystem.Data.Models
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	using JudgeSystem.Common;
	using JudgeSystem.Data.Common.Models;

	public class Problem : BaseDeletableModel<int>
    {
		public Problem()
		{
			this.Tests = new HashSet<Test>();
			this.Submissions = new HashSet<Submission>();
		}

		[Required]
		[MinLength(GlobalConstants.NameMinLength)]
		public string Name { get; set; }

		public bool IsExtraTask { get; set; }

		public int MaxPoints { get; set; }

		public int LessonId { get; set; }
		public Lesson Lesson { get; set; }

		public ICollection<Test> Tests { get; set; }

		public ICollection<Submission> Submissions { get; set; }
	}
}
