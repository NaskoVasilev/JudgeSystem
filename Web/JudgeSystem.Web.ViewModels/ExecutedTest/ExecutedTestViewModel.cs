namespace JudgeSystem.Web.ViewModels.ExecutedTest
{
	using JudgeSystem.Data.Models.Enums;
	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Data.Models;
	using AutoMapper;

	public class ExecutedTestViewModel : IMapFrom<ExecutedTest>
	{
		public int Id { get; set; }

		public bool IsCorrect { get; set; }

		public string Output { get; set; }

		public string Error { get; set; }

		public string TestInputData { get; set; }

		public string TestOutputData { get; set; }

		public bool TestIsTrialTest { get; set; }

		public double TimeUsed { get; set; }

		public double MemoryUsed { get; set; }

		public TestExecutionResultType ExecutionResultType { get; set; }

		[IgnoreMap]
		public string ExecutionResult
		{
			get
			{
				string exectionResult = string.Empty;
				if(ExecutionResultType == TestExecutionResultType.MemoryLimit)
				{
					exectionResult = "Memory limit";
				}
				else if(ExecutionResultType == TestExecutionResultType.RunTimeError)
				{
					exectionResult = "Run time error";
				}
				else if(ExecutionResultType == TestExecutionResultType.TimeLimit)
				{
					exectionResult = "Time limit";
				}

				else if(ExecutionResultType == TestExecutionResultType.Success && this.IsCorrect)
				{
					exectionResult = "Correct answer";
				}
				else{
					exectionResult = "Incorrect answer";
				}

				return exectionResult;
			}
		}
	}
}
