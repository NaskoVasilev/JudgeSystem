namespace JudgeSystem.Web.Dtos.Submission
{
	using JudgeSystem.Web.Dtos.ExecutedTest;

	using System.Collections.Generic;

	public class SubmissionResult
	{
		public SubmissionResult()
		{
			ExecutedTests = new List<ExecutedTestResult>();
			IsCompiledSuccessfully = true;
		}

		public int Id { get; set; }

		public bool IsCompiledSuccessfully { get; set; }

		public ICollection<ExecutedTestResult> ExecutedTests { get; set; }

		public int MaxPoints { get; set; }

		public int ActualPoints { get; set; }

		public string SubmissionDate { get; set; }
	}
}
