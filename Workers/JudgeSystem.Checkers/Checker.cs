using System.Threading.Tasks;

using JudgeSystem.Executors;
using JudgeSystem.Workers.Common;

namespace JudgeSystem.Checkers
{
	public class Checker
	{
		private readonly IExecutor executor;

		public Checker(IExecutor executor)
		{
            this.executor = executor;
		}

		public async Task<CheckerResult> Check(string filePath, string input, string expectedOutput, int timeLimit, int memoryLimit)
		{
			ExecutionResult executionResult = await executor.Execute(filePath, input, timeLimit, memoryLimit);
			var checkerResult = new CheckerResult(executionResult);
			if (!executionResult.IsSuccesfull)
			{
				checkerResult.IsCorrect = false;
				return checkerResult;
			}

			if(executionResult.Output.Trim() == expectedOutput.Trim())
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
