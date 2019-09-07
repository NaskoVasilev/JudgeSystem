using System.Diagnostics;
using System.Threading.Tasks;

using JudgeSystem.Workers.Common;

namespace JudgeSystem.Executors
{
    public class CSharpExecutor : IExecutor
    {
        public Task<ExecutionResult> Execute(string filePath, string input, int timeLimit, int memoryLimit)
        {
            string arguments = $"{CompilationSettings.ConsoleComamndPrefix} dotnet {filePath}";
            var executor = new Executor();
            return executor.Execute(arguments, input, timeLimit, memoryLimit);
        }
    }
}
