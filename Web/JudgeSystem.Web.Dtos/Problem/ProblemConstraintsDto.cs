namespace JudgeSystem.Web.Dtos.Problem
{
    using JudgeSystem.Services.Mapping;
    using JudgeSystem.Data.Models;

    public class ProblemConstraintsDto : IMapFrom<Problem>
    {
        public int AllowedTimeInMilliseconds { get; set; }

        public double AllowedMemoryInMegaBytes { get; set; }
    }
}
