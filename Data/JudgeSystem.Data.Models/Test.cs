using JudgeSystem.Data.Common.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JudgeSystem.Data.Models
{
	public class Test : IOrderable
	{
		public Test()
		{
			IsTrialTest = false;
			ExecutedTests = new HashSet<ExecutedTest>();
		}

		public int Id { get; set; }

        public int OrderBy { get; set; }

        public Problem Problem { get; set; }
		public int ProblemId { get; set; }

		public string InputData { get; set; }

		[Required]
		public string OutputData { get; set; }

		public bool IsTrialTest { get; set; }

		public ICollection<ExecutedTest> ExecutedTests { get; set; }
	}
}
