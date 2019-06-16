namespace JudgeSystem.Data.Models
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class Test
	{
		public Test()
		{
			IsTrialTest = false;
			this.ExecutedTests = new HashSet<ExecutedTest>();
		}

		public int Id { get; set; }

		public Problem Problem { get; set; }
		public int ProblemId { get; set; }

		public string InputData { get; set; }

		[Required]
		public string OutputData { get; set; }

		public bool IsTrialTest { get; set; }

		public ICollection<ExecutedTest> ExecutedTests { get; set; }
	}
}
