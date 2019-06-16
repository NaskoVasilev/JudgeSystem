namespace JudgeSystem.Web.ViewModels.Submission
{
	using System.Collections.Generic;

	using JudgeSystem.Web.ViewModels.ExecutedTest;

	public class SubmissionResult
	{
		public SubmissionResult()
		{
			this.ExecutedTests = new List<ExecutedTestResult>();
			this.IsCompiledSuccessfully = true;
		}

		public bool IsCompiledSuccessfully { get; set; }

		public ICollection<ExecutedTestResult> ExecutedTests { get; set; }
	}
}
