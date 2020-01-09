using System.Collections.Generic;

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

        public static IEnumerable<Test> GetEntities()
        {
            Problem problem = ProblemTestData.GetEntity();

            return new List<Test>()
            {
                new Test
                {
                    Id = 1,
                    InputData = "input1",
                    OutputData = "output1",
                    IsTrialTest = true,
                    ProblemId = problem.Id
                },
                new Test
                {
                    Id = 2,
                    InputData = "input2",
                    OutputData = "output2",
                    IsTrialTest = false,
                    ProblemId = problem.Id
                }
            };
        }
    }
}
