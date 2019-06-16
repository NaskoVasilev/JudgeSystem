using System.ComponentModel.DataAnnotations;

namespace JudgeSystem.Web.InputModels.Submission
{
	public class SubmissionInputModel
	{
		[Required]
		[MinLength(10)]
		public string Code { get; set; }

		public int ProblemId { get; set; }
	}
}
