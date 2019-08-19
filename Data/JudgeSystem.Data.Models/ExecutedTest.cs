using JudgeSystem.Data.Common.Models;
using JudgeSystem.Data.Models.Enums;

namespace JudgeSystem.Data.Models
{
	public class ExecutedTest : BaseModel<int>
	{
		public ExecutedTest()
		{
			IsCorrect = false;
			ExecutionResultType = TestExecutionResultType.Success;
		}

		public bool IsCorrect { get; set; }

		public string Output { get; set; }

		public string Error { get; set; }

		public int TestId { get; set; }
		public Test Test { get; set; }

		public int SubmissionId { get; set; }
		public Submission Submission { get; set; }

		public double TimeUsed { get; set; }

		public double MemoryUsed { get; set; }

		public TestExecutionResultType ExecutionResultType { get; set; }
	}
}
