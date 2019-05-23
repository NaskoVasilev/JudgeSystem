namespace JudgeSystem.Web.ViewModels.Test
{
	using Services.Mapping;
	using Data.Models;

	public class TestEditInputModel : IMapTo<Test>, IMapFrom<Test>
	{
		public int Id { get; set; }

		public string InputData { get; set; }

		public string OutputData { get; set; }
	}
}
