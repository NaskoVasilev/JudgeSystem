namespace JudgeSystem.Web.ViewModels.ExecutedTest
{
	using JudgeSystem.Data.Models.Enums;
	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Data.Models;

	public class ExecutedTestViewModel : IMapFrom<ExecutedTest>
	{
		public bool IsCorrect { get; set; }

		public string Output { get; set; }

		public string Error { get; set; }

		public string TestInputData { get; set; }

		public string TestOutputData { get; set; }

		public bool TestIsTrialTest { get; set; }

		public double TimeUsed { get; set; }

		public double MemoryUsed { get; set; }

		public TestExecutionResultType ExecutionResultType { get; set; }
	}
}
