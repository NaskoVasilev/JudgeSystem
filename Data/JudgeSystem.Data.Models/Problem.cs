using System.Collections.Generic;

namespace JudgeSystem.Data.Models
{
    public class Problem
    {
		public Problem()
		{
			this.Tests = new List<Test>();
		}

		public int Id { get; set; }

		public string Name { get; set; }

		public bool IsExtraTask { get; set; }

		public int MaxPoints { get; set; }

		public int LessonId { get; set; }
		public Lesson Lesson { get; set; }

		public ICollection<Test> Tests { get; set; }
	}
}
