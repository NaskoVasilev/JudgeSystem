using System.ComponentModel.DataAnnotations;

namespace JudgeSystem.Web.ViewModels.Test
{
	public class TestInputModel
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
