using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using JudgeSystem.Common;
using JudgeSystem.Data.Common.Models;

namespace JudgeSystem.Data.Models
{
	public class Contest : BaseDeletableModel<int>
	{
		public Contest()
		{
			this.UserContests = new HashSet<UserContest>();
			this.Submissions = new HashSet<Submission>();
		}

		[Required]
		[MaxLength(ModelConstants.ContestNameMaxLength)]
		public string Name { get; set; }

		public DateTime StartTime { get; set; }

		public DateTime EndTime { get; set; }

		[NotMapped]
		public bool IsActive => StartTime < DateTime.Now && EndTime > DateTime.Now;

		public int LessonId { get; set; }
		public Lesson Lesson { get; set; }

		public ICollection<UserContest> UserContests { get; set; }

		public ICollection<Submission> Submissions { get; set; }
	}
}
