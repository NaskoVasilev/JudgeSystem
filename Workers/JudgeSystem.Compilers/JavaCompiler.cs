using System.Collections.Generic;

using JudgeSystem.Common;
using JudgeSystem.Workers.Common;

namespace JudgeSystem.Compilers
{
    public class JavaCompiler : ICompiler
    {
        public CompileResult Compile(string fileName, string workingDirectory, IEnumerable<string> sources = null)
        {
            //command: /c cd C://CompiledSolutions & set PATH=%PATH%;C:\\Program Files\\Java\\jdk1.8.0_181\\bin; & javac Task.java & jar cvfe Task.jar Task Task.class
            string outputFile = $"{fileName}{CompilationSettings.JavaOutputFileExtension}";
            string outputFilePath = workingDirectory + outputFile;
            string sourceFile = $"{fileName}{GlobalConstants.JavaFileExtension}";
            string arguments = $"/c cd {workingDirectory} & set PATH=%PATH%;{CompilationSettings.JavaCompilerPath}; & javac {sourceFile} & jar cvfe {outputFile} {fileName} {fileName}.class";

            var compiler = new Compiler();
            CompileResult result = compiler.Compile(arguments);
            result.OutputFilePath = outputFilePath;
            return result;
        }
    }
}
