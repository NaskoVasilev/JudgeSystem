using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.Dtos.Problem
{
    public class ProblemSubmissionDto : IMapFrom<Data.Models.Problem>
    {
        public TestingStrategy TestingStrategy { get; set; }

        public byte[] AutomatedTestingProject { get; set; }

        public int TimeIntervalBetweenSubmissionInSeconds { get; set; }

        public int AllowedMinCodeDifferenceInPercentage { get; set; }

        public SubmissionType SubmissionType { get; set; }
    }
}
