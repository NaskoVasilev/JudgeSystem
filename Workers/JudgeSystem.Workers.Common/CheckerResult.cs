namespace JudgeSystem.Workers.Common
{
	public class CheckerResult : ExecutionResult
	{
		public CheckerResult(ExecutionResult executionResult)
		{
			Error = executionResult.Error;
			Output = executionResult.Output;
			ExitCode = executionResult.ExitCode;
			MemoryUsed = executionResult.MemoryUsed;
			PrivilegedProcessorTime = executionResult.PrivilegedProcessorTime;
            TimeWorked = executionResult.TimeWorked;
			Type = executionResult.Type;
			UserProcessorTime = executionResult.UserProcessorTime;
		}

		public bool IsCorrect { get; set; }
	}
}
