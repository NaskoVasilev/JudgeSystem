using System.Collections.Generic;

using JudgeSystem.Common;
using JudgeSystem.Workers.Common;

namespace JudgeSystem.Compilers
{
    public class CPlusPlusCompiler : ICompiler
    {
        public CompileResult Compile(string fileName, string workingDirectory, IEnumerable<string> sources = null)
        {
            //command: /c cd C://CompiledSolutions & set PATH=%PATH%;C:\\MinGW\\bin; & g++ Task.cpp -o Task.exe"
            string outputFile = $"{fileName}{CompilationSettings.ExeFileExtension}";
            string outputFilePath = workingDirectory + outputFile;
            string sourceFile = $"{fileName}{GlobalConstants.CppFileExtension}";
            //string arguments = $"{CompilationSettings.ConsoleComamndPrefix} cd {workingDirectory}{CompilationSettings.SetCPlusPlusCompilerPathCommand} & g++ {sourceFile} -o {outputFile}";
            string arguments = $"{CompilationSettings.ConsoleComamndPrefix} cd {workingDirectory}{CompilationSettings.SetCPlusPlusCompilerPathCommand} & cl /EHsc {sourceFile}";

            var compiler = new Compiler();
            CompileResult result = compiler.Compile(arguments);
            result.OutputFilePath = outputFilePath;
            return result;
        }
    }
}
