using JudgeSystem.Executors;
using JudgeSystem.Workers.Common;

namespace JudgeSystem.Checkers
{
	public class CSharpChecker
	{
		private readonly CSharpExecutor cSharpExecutor;

		public CSharpChecker()
		{
			this.cSharpExecutor = new CSharpExecutor();
		}

		public CheckerResult Check(string dllFilePath, string input, string expectedOutput)
		{
			ExecutionResult executionResult = cSharpExecutor.ProcessExecutionResult(dllFilePath, input);
			if (!executionResult.IsSuccesfull)
			{
				return new CheckerResult { IsCorrect = false, ErrorMessage = executionResult.Error, HasRuntimeError = true };
			}

			CheckerResult checkerResult = new CheckerResult() { Output = executionResult.Output };
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
