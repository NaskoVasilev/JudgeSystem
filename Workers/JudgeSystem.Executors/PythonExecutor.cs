using System;
using System.Threading.Tasks;
using JudgeSystem.Workers.Common;

namespace JudgeSystem.Executors
{
    public class PythonExecutor : IExecutor
    {
        public Task<ExecutionResult> Execute(string filePath, string input, int timeLimit, int memoryLimit)
        {
            var baseExecutor = new Executor();
            string arguments = "Place Python program execution arguments here";
            return baseExecutor.Execute(arguments, input, timeLimit, memoryLimit);
        }
    }
}
