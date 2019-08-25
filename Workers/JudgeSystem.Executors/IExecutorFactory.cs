using JudgeSystem.Workers.Common;

namespace JudgeSystem.Executors
{
    public interface IExecutorFactory
    {
        IExecutor CreateExecutor(ProgrammingLanguage programmingLanguage);
    }
}
