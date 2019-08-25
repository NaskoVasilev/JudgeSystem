using JudgeSystem.Workers.Common;

namespace JudgeSystem.Compilers
{
    public interface ICompilerFactory
    {
        ICompiler CreateCompiler(ProgrammingLanguage programmingLanguage);
    }
}
