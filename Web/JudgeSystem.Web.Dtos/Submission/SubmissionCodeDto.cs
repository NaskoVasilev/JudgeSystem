using System.Collections.Generic;

namespace JudgeSystem.Web.Dtos.Submission
{
    public class SubmissionCodeDto
    {
        public List<CodeFile> SourceCodes { get; set; }

        public byte[] Content { get; set; }
    }
}
