using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using JudgeSystem.Common;
using JudgeSystem.Data.Common.Models;

namespace JudgeSystem.Data.Models
{
	public class Course : BaseDeletableModel<int>
	{
		public Course()
		{
			Lessons = new HashSet<Lesson>();
		}

		[Required]
		[MaxLength(ModelConstants.CourseNameMaxLength)]
		public string Name { get; set; }

        public int OrderBy { get; set; }

        public ICollection<Lesson> Lessons { get; set; }
	}
}
