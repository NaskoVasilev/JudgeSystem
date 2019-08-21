using System;

namespace JudgeSystem.Workers.Common
{
	public class ExecutionResult
	{
		public ExecutionResult()
		{
			Output = string.Empty;
			Error = string.Empty;
			ExitCode = 0;
			Type = ProcessExecutionResultType.Success;
			TimeWorked = default;
			MemoryUsed = 0;
		}

		public string Output { get; set; }

		public string Error { get; set; }

		public bool IsSuccesfull => string.IsNullOrEmpty(Error);

		public int ExitCode { get; set; }

		public ProcessExecutionResultType Type { get; set; }

		public TimeSpan TimeWorked { get; set; }

		public TimeSpan PrivilegedProcessorTime { get; set; }

		public TimeSpan UserProcessorTime { get; set; }

		public long MemoryUsed { get; set; }

		public TimeSpan TotalProcessorTime => PrivilegedProcessorTime + UserProcessorTime;
	}
}
