using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;

namespace JudgeSystem.Web.Tests.TestData
{
    public class ExecutedTestTestData
    {
        public static ExecutedTest GetEntity() => new ExecutedTest()
        {
            Id = 1,
            Submission = SubmissionTestData.GetEntity(),
            Output = "some output",
            MemoryUsed = 12,
            TimeUsed = 200,
            ExecutionResultType = TestExecutionResultType.Success,
            Test = TestTestData.GetEntity()
        };
    }
}
