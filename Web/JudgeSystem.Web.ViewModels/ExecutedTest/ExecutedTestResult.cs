namespace JudgeSystem.Web.ViewModels.ExecutedTest
{
	public class ExecutedTestResult
	{
		public ExecutedTestResult()
		{
			this.ExecutedSuccessfully = true;
			this.IsCorrect = false;
		}

		public bool IsCorrect { get; set; }

		public bool ExecutedSuccessfully { get; set; }
	}
}
