using System.IO;
using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Workers.Common;

namespace JudgeSystem.Executors
{
    public class CPlusPlusExecutor : IExecutor
    {
        public Task<ExecutionResult> Execute(string filePath, string input, int timeLimit, int memoryLimit)
        {
            string fileName = Path.GetFileName(filePath);
            string workingDirectory = Path.GetDirectoryName(filePath);
            string arguments = $"{GlobalConstants.ConsoleComamndPrefix} cd {workingDirectory}{CompilationSettings.SetCPlusPlusCompilerPathCommand} & {fileName}";
            var executor = new Executor();
            return executor.Execute(arguments, input, timeLimit, memoryLimit);
        }
    }
}
