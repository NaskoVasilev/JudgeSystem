namespace JudgeSystem.Data.Models
{
	public class Test
	{
		public int Id { get; set; }

		public int ProblemId { get; set; }
		public Problem Problem { get; set; }

		public string InputData { get; set; }

		public string OutputData { get; set; }

		public bool IsTrialTest { get; set; }
	}
}
