using System.ComponentModel.DataAnnotations;

using JudgeSystem.Common;
using JudgeSystem.Workers.Common;

using Microsoft.AspNetCore.Http;

namespace JudgeSystem.Web.InputModels.Submission
{
    public class SubmissionInputModel
	{
        [MinLength(GlobalConstants.MinSubmissionCodeLength)]
		public string Code { get; set; }

        public ProgrammingLanguage ProgrammingLanguage { get; set; }

        public int ProblemId { get; set; }

		public int? ContestId { get; set; }

        public int? PracticeId { get; set; }

        public IFormFile File { get; set; }

        public byte[] SubmissionContent { get; set; }
    }
}
