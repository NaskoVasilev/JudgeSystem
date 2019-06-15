namespace JudgeSystem.Workers.Common
{
	public class ExecutionResult
	{
		public string Output { get; set; }

		public string Error { get; set; }

		public bool IsSuccesfull => string.IsNullOrEmpty(Error);
	}
}
