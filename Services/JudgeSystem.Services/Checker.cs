using JudgeSystem.Workers.Common;

namespace JudgeSystem.Services
{
    public class Checker : IChecker
    {
        public CheckerResult Check(ExecutionResult executionResult, string expectedOutput)
        {
            var checkerResult = new CheckerResult(executionResult);
            if (!executionResult.IsSuccesfull)
            {
                checkerResult.IsCorrect = false;
                return checkerResult;
            }

            if (executionResult.Output.Trim() == expectedOutput.Trim())
            {
                checkerResult.IsCorrect = true;
            }
            else
            {
                checkerResult.IsCorrect = false;
            }

            return checkerResult;
        }
    }
}
