using System.Collections.Generic;

using JudgeSystem.Common;
using JudgeSystem.Workers.Common;

namespace JudgeSystem.Compilers
{
    public class JavaCompiler : ICompiler
    {
        public CompileResult Compile(string fileName, string workingDirectory, IEnumerable<string> sources = null)
        {
            //command to compile and create .jar file: /c cd {workingDirectory} & set PATH=%PATH%;{CompilationSettings.JavaCompilerPath}; & javac {sourceFile} & jar cvfe {outputFile}
            //command to compile and create .jar file example: /c cd C:\\CompiledSolutions & set PATH=%PATH%;C:\\Program Files\\Java\\jdk1.8.0_181\\bin; & javac Task.java & jar cvfe Task.jar Task Task.class
            //command: /c cd [working folder] & set PATH=%PATH%;[path to compiler]; & javac [fileName with java extension]
            //command example: /c cd C:\\Solutions & set PATH=%PATH%;C:\\Program Files\\Java\\jdk1.8.0_181\\bin; & javac StartUp.java

            string outputFile = $"{fileName}{GlobalConstants.JavaFileExtension}";
            string outputFilePath = workingDirectory + outputFile;
            string sourceFile = $"{fileName}{GlobalConstants.JavaFileExtension}";
            string arguments = $"{CompilationSettings.ConsoleComamndPrefix} cd {workingDirectory} & set PATH=%PATH%;{CompilationSettings.JavaCompilerPath}; & javac {sourceFile}";

            var compiler = new Compiler();
            CompileResult result = compiler.Compile(arguments);
            result.OutputFilePath = outputFilePath;
            return result;
        }
    }
}
