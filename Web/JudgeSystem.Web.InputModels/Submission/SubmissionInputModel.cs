using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace JudgeSystem.Web.InputModels.Submission
{
	public class SubmissionInputModel
	{
		public string Code { get; set; }

		public int ProblemId { get; set; }

		public int? ContestId { get; set; }

        public IFormFile File { get; set; }

        public byte[] SubmissionContent { get; set; }
    }
}
