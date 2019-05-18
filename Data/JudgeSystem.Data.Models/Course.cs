using JudgeSystem.Data.Common.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JudgeSystem.Data.Models
{
	public class Course : BaseDeletableModel<int>
	{
		public Course()
		{
			this.Lessons = new List<Lesson>();
		}

		[Required]
		[MinLength(3)]
		public string Name { get; set; }

		public ICollection<Lesson> Lessons { get; set; }
	}
}
