using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.ViewModels.Test
{
    public class TestViewModel : IMapFrom<Data.Models.Test>
	{
		public int Id { get; set; }

		public string InputData { get; set; }

		public string OutputData { get; set; }

		public bool IsTrialTest { get; set; }
	}
}
