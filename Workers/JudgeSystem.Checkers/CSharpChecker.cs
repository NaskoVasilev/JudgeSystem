using System.Threading.Tasks;

using JudgeSystem.Executors;
using JudgeSystem.Workers.Common;

namespace JudgeSystem.Checkers
{
	public class CSharpChecker
	{
		private readonly CSharpExecutor cSharpExecutor;

		public CSharpChecker()
		{
            cSharpExecutor = new CSharpExecutor();
		}

		public async Task<CheckerResult> Check(string dllFilePath, string input, string expectedOutput, int timeLimit, int memoryLimit)
		{
			ExecutionResult executionResult = await cSharpExecutor.ProcessExecutionResult(dllFilePath, input, timeLimit, memoryLimit);
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
