using JudgeSystem.Data.Common.Models;
using System.Collections.Generic;

namespace JudgeSystem.Data.Models
{
	public class Course : BaseDeletableModel<int>
	{
		public Course()
		{
			this.Lessons = new List<Lesson>();
		}

		public string Name { get; set; }

		public ICollection<Lesson> Lessons { get; set; }
	}
}
