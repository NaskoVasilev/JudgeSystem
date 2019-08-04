using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.Dtos.Problem
{
    public class ProblemConstraintsDto : IMapFrom<Data.Models.Problem>
    {
        public int AllowedTimeInMilliseconds { get; set; }

        public double AllowedMemoryInMegaBytes { get; set; }
    }
}
