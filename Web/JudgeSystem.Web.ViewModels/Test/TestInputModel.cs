namespace JudgeSystem.Web.ViewModels.Test
{
	using System.ComponentModel.DataAnnotations;

	using Services.Mapping;
	using Data.Models;

	public class TestInputModel : IMapTo<Test>
	{
		public int Id { get; set; }
		
		public int ProblemId { get; set; }

		[Required]
		public string InputData { get; set; }

		[Required]
		public string OutputData { get; set; }

		public bool IsTrialTest { get; set; }
	}
}
