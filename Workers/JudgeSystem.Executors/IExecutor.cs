using JudgeSystem.Workers.Common;
using System.Threading.Tasks;

namespace JudgeSystem.Executors
{
    public interface IExecutor
    {
        Task<ExecutionResult> Execute(string filePath, string input, int timeLimit, int memoryLimit);
    }
}
