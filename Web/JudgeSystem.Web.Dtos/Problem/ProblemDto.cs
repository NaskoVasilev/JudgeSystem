using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.Dtos.Problem
{
    public class ProblemDto : IMapFrom<Data.Models.Problem>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsExtraTask { get; set; }

        public int MaxPoints { get; set; }

        public int AllowedTimeInMilliseconds { get; set; }

        public double AllowedMemoryInMegaBytes { get; set; }

        public int LessonId { get; set; }

        public SubmissionType SubmissionType { get; set; }
    }
}
