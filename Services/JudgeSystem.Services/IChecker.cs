using JudgeSystem.Workers.Common;

namespace JudgeSystem.Services
{
    public interface IChecker
    {
        CheckerResult Check(ExecutionResult executionResult, string expectedOutput);
    }
}
