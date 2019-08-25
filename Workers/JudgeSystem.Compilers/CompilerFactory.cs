using System;

using JudgeSystem.Workers.Common;

namespace JudgeSystem.Compilers
{
    public class CompilerFactory : ICompilerFactory
    {
        private const string UnsupportedCompilerErrorMessage = "Unsupported compiler.";

        public ICompiler CreateCompiler(ProgrammingLanguage programmingLanguage)
        {
            switch (programmingLanguage)
            {
                case ProgrammingLanguage.CSharp:
                    return new CSharpCompiler();
                case ProgrammingLanguage.CPlusPlus:
                    return new CPlusPlusCompiler();
                case ProgrammingLanguage.Java:
                    return new JavaCompiler();
                default:
                    throw new ArgumentException(UnsupportedCompilerErrorMessage);
            }
        }
    }
}
