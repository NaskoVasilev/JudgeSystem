namespace JudgeSystem.Web.Dtos.ExecutedTest
{
	public class ExecutedTestResult
	{
		public ExecutedTestResult()
		{
			ExecutedSuccessfully = true;
			IsCorrect = false;
		}

		public bool IsCorrect { get; set; }

		public bool ExecutedSuccessfully { get; set; }
	}
}
