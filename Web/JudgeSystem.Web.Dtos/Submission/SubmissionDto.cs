using System;

using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.Dtos.Submission
{
    public class SubmissionDto : IMapFrom<Data.Models.Submission>
    {
        public int Id { get; set; }

        public byte[] Code { get; set; }

        public byte[] CompilationErrors { get; set; }

        public int ActualPoints { get; set; }

        public int ProblemId { get; set; }

        public int? ContestId { get; set; }

        public int? PracticeId { get; set; }

        public DateTime SubmisionDate { get; set; }

        public string UserId { get; set; }
    }
}
