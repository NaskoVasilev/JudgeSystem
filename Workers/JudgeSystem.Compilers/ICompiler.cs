using System.Collections.Generic;

using JudgeSystem.Workers.Common;

namespace JudgeSystem.Compilers
{
    public interface ICompiler
    {
        CompileResult Compile(string fileName, string workingDirectory, IEnumerable<string> sources = null);
    }
}
