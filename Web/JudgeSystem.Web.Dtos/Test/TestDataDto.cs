using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.Dtos.Test
{
    public class TestDataDto : IMapFrom<Data.Models.Test>
	{
		public int Id { get; set; }

		public string InputData { get; set; }

		public string OutputData { get; set; }
	}
}
