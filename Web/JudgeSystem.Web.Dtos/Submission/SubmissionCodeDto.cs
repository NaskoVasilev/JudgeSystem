using System.Collections.Generic;

namespace JudgeSystem.Web.Dtos.Submission
{
    public class SubmissionCodeDto
    {
        public List<string> SourceCodes { get; set; }

        public byte[] Content { get; set; }
    }
}
