namespace JudgeSystem.Data.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public class Submission
	{
		public Submission()
		{
			this.SubmisionDate = DateTime.Now;
		}

		public int Id { get; set; }

		[Required]
		public byte[] Code { get; set; }

		public byte[] CompilationErrors { get; set; }

		public int ProblemId { get; set; }
		public Problem Problem { get; set; }

		public DateTime SubmisionDate { get; set; }

		public string UserId { get; set; }
		public ApplicationUser User { get; set; }
	}
}
