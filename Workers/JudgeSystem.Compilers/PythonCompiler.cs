using System.Collections.Generic;

using JudgeSystem.Workers.Common;

namespace JudgeSystem.Compilers
{
    public class PythonCompiler : ICompiler
    {
        public CompileResult Compile(string fileName, string workingDirectory, IEnumerable<string> sources = null)
        {
            var baseCompiler = new Compiler();
            string arguments = "Place Python compilation arguments here";
            return baseCompiler.Compile(arguments);
        }
    }
}
