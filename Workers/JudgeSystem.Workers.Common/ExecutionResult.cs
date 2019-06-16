using System;

namespace JudgeSystem.Workers.Common
{
	public class ExecutionResult
	{
		public ExecutionResult()
		{
			this.Output = string.Empty;
			this.Error = string.Empty;
			this.ExitCode = 0;
			this.Type = ProcessExecutionResultType.Success;
			this.TimeWorked = default(TimeSpan);
			this.MemoryUsed = 0;
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

		public TimeSpan TotalProcessorTime => this.PrivilegedProcessorTime + this.UserProcessorTime;
	}
}
