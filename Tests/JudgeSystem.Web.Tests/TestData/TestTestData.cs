using JudgeSystem.Data.Models;

namespace JudgeSystem.Web.Tests.TestData
{
    public class TestTestData
    {
        public static Test GetEntity() => new Test
        {
            Id = 1,
            InputData = "input data",
            OutputData = "output data",
            IsTrialTest = false,
            Problem = ProblemTestData.GetEntity()
        };
    }
}
