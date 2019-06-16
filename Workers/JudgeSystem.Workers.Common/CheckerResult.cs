namespace JudgeSystem.Workers.Common
{
	public class CheckerResult : ExecutionResult
	{
		public CheckerResult(ExecutionResult executionResult)
		{
			this.Error = executionResult.Error;
			this.Output = executionResult.Output;
			this.ExitCode = executionResult.ExitCode;
			this.MemoryUsed = executionResult.MemoryUsed;
			this.PrivilegedProcessorTime = executionResult.PrivilegedProcessorTime;
			this.TimeWorked = executionResult.TimeWorked;
			this.Type = executionResult.Type;
			this.UserProcessorTime = executionResult.UserProcessorTime;
		}

		public bool IsCorrect { get; set; }
	}
}
