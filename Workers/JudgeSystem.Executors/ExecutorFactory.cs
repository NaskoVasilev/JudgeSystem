using System;

using JudgeSystem.Workers.Common;

namespace JudgeSystem.Executors
{
    public class ExecutorFactory : IExecutorFactory
    {
        private const string UnsupportedExecutorErrorMessage = "Unsupported executor.";

        public IExecutor CreateExecutor(ProgrammingLanguage programmingLanguage)
        {
            switch (programmingLanguage)
            {
                case ProgrammingLanguage.CSharp:
                    return new CSharpExecutor();
                case ProgrammingLanguage.CPlusPlus:
                    return new CPlusPlusExecutor();
                case ProgrammingLanguage.Java:
                    return new JavaExecutor();
                case ProgrammingLanguage.Python:
                    return new PythonExecutor();
                default:
                    throw new ArgumentException(UnsupportedExecutorErrorMessage);
            }
        }
    }
}
