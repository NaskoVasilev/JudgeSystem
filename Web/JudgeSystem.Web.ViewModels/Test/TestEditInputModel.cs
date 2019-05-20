namespace JudgeSystem.Web.ViewModels.Test
{
	using JudgeSystem.Services.Mapping;
	using System.ComponentModel.DataAnnotations;
	using Data.Models;

	public class TestEditInputModel : IMapTo<Test>, IMapFrom<Test>
	{
		public int Id { get; set; }

		[Required]
		public string InputData { get; set; }

		[Required]
		public string OutputData { get; set; }

		public bool IsTrialTest { get; set; }
	}
}
