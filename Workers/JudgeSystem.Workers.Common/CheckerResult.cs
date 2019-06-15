namespace JudgeSystem.Workers.Common
{
	public class CheckerResult
	{
		public string Output { get; set; }

		public bool IsCorrect { get; set; }

		public bool HasRuntimeError { get; set; }

		public string ErrorMessage { get; set; }
	}
}
