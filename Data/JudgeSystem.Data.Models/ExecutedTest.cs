namespace JudgeSystem.Data.Models
{
	using JudgeSystem.Data.Common.Models;

	public class ExecutedTest : BaseModel<int>
	{
		public ExecutedTest()
		{
			this.ExecutedSuccessfully = true;
			this.IsCorrect = false;
		}

		public bool IsCorrect { get; set; }

		public string Output { get; set; }

		public int TestId { get; set; }
		public Test Test { get; set; }

		public bool ExecutedSuccessfully { get; set; }
	}
}
