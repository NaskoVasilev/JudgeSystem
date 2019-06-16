using JudgeSystem.Executors;
using JudgeSystem.Workers.Common;
using System.Threading.Tasks;

namespace JudgeSystem.Checkers
{
	public class CSharpChecker
	{
		private readonly CSharpExecutor cSharpExecutor;

		public CSharpChecker()
		{
			this.cSharpExecutor = new CSharpExecutor();
		}

		public async Task<CheckerResult> Check(string dllFilePath, string input, string expectedOutput)
		{
			ExecutionResult executionResult = await cSharpExecutor.ProcessExecutionResult(dllFilePath, input);
			CheckerResult checkerResult = new CheckerResult(executionResult);
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
