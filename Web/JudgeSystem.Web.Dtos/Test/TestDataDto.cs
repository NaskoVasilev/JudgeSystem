namespace JudgeSystem.Web.Dtos.Test
{
    using JudgeSystem.Services.Mapping;
    using JudgeSystem.Data.Models;

    public class TestDataDto : IMapFrom<Test>
	{
		public int Id { get; set; }

		public string InputData { get; set; }

		public string OutputData { get; set; }
	}
}
