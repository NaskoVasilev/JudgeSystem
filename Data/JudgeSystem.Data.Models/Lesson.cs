using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using JudgeSystem.Common;
using JudgeSystem.Data.Common.Models;
using JudgeSystem.Data.Models.Enums;

namespace JudgeSystem.Data.Models
{
	public class Lesson : BaseDeletableModel<int>, IOrderable
	{
		public Lesson()
		{
			Resources = new HashSet<Resource>();
			Problems = new HashSet<Problem>();
			Contests = new HashSet<Contest>();
		}

		[Required]
        [MaxLength(ModelConstants.LessonNameMaxLength)]
		public string Name { get; set; }

        public int OrderBy { get; set; }

        public int CourseId { get; set; }
		public Course Course { get; set; }

		public string LessonPassword { get; set; }

		[NotMapped]
		public bool IsLocked => LessonPassword != null;

		public LessonType Type { get; set; }

        public Practice Practice { get; set; }

        public ICollection<Resource> Resources { get; set; }

		public ICollection<Problem> Problems { get; set; }

		public ICollection<Contest> Contests { get; set; }
    }
}
