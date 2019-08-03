using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using JudgeSystem.Data.Common.Models;

namespace JudgeSystem.Data.Models
{
	public class Submission : BaseModel<int>
	{
		public Submission()
		{
			this.SubmisionDate = DateTime.Now;
			this.ExecutedTests = new HashSet<ExecutedTest>();
			this.ActualPoints = 0;
		}

		[Required]
		public byte[] Code { get; set; }

		public byte[] CompilationErrors { get; set; }

		public int ActualPoints { get; set; }

		public int ProblemId { get; set; }
		public Problem Problem { get; set; }

		public int? ContestId { get; set; }
		public Contest Contest { get; set; }

        public int? PracticeId { get; set; }
        public Practice Practice { get; set; }

        public DateTime SubmisionDate { get; set; }

		public string UserId { get; set; }
		public ApplicationUser User { get; set; }

		public ICollection<ExecutedTest> ExecutedTests { get; set; }
	}
}
