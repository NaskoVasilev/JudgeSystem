using JudgeSystem.Data.Models.Enums;

namespace JudgeSystem.Web.Dtos.ExecutedTest
{
	public class ExecutedTestResult
	{
		public ExecutedTestResult()
		{
			IsCorrect = false;
		}

		public bool IsCorrect { get; set; }

		public string ExecutionResultType { get; set; }
	}
}
