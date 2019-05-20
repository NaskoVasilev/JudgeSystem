namespace JudgeSystem.Web.ViewModels.Test
{
	using Services.Mapping;
	using Data.Models;

	public class TestViewModel : IMapTo<Test>
	{
		public int Id { get; set; }

		public string InputData { get; set; }

		public string OutputData { get; set; }

		public bool IsTrialTest { get; set; }
	}
}
