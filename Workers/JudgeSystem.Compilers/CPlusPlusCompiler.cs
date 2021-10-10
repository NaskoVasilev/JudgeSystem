using System.Collections.Generic;
using System.Runtime.InteropServices;
using JudgeSystem.Common;
using JudgeSystem.Workers.Common;

namespace JudgeSystem.Compilers
{
    public class CPlusPlusCompiler : ICompiler
    {
        public CompileResult Compile(string fileName, string workingDirectory, IEnumerable<string> sources = null)
        {
            // g++ ccdtzur2.dad.cpp -o test
            // ./ test
            //command: /c cd C://CompiledSolutions & set PATH=%PATH%;C:\\MinGW\\bin; & g++ Task.cpp -o Task.exe
            string outputFile = $"{workingDirectory}{fileName}";
            string sourceFile = $"{workingDirectory}{fileName}{GlobalConstants.CppFileExtension}";
            string arguments = $"{sourceFile} -o {outputFile}";
            string processFileName = "g++";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                outputFile += CompilationSettings.ExeFileExtension;
                arguments = $"{GlobalConstants.ConsoleComamndPrefix}{CompilationSettings.SetCPlusPlusCompilerPathCommand} & g++ {arguments}";
                processFileName = GlobalConstants.ConsoleFile;
            }

            var compiler = new Compiler();
            CompileResult result = compiler.Compile(arguments, processFileName);
            result.OutputFilePath = outputFile;
            return result;
        }
    }
}
