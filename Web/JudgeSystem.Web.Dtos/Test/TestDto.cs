using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.Dtos.Test
{
    public class TestDto : IMapFrom<Data.Models.Test>
    {
        public int Id { get; set; }

        public int ProblemId { get; set; }

        public string InputData { get; set; }

        public string OutputData { get; set; }

        public bool IsTrialTest { get; set; }
    }
}
