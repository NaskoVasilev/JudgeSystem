using System.IO;
using System.Threading.Tasks;

using JudgeSystem.Workers.Common;

namespace JudgeSystem.Executors
{
    public class JavaExecutor : IExecutor
    {
        public Task<ExecutionResult> Execute(string filePath, string input, int timeLimit, int memoryLimit)
        {
            //command: /c cd [workingFolder] & set PATH=%PATH%;[Java Compiler Path(jdk bin folder)]; & java [ClassWhereIsMainMethodFileNameWithoutExtension] -Xmx128m
            //example: /c cd C:\Solutions & set PATH=%PATH%;C:\Program Files\Java\jdk1.8.0_181\bin; & java StartUp -Xmx128m
            //command to execute jar file: "/c cd {workingDirectory} & set PATH=%PATH%;{CompilationSettings.JavaCompilerPath}; & java -Xmx128m -jar -Djava.security.manager -Djava.security.policy=={fileNameWithoutExtension}.policy {fileName}";

            string workingDirectory = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileName(filePath);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string arguments = $"/c cd {workingDirectory} & set PATH=%PATH%;{CompilationSettings.JavaCompilerPath}; & java {fileNameWithoutExtension} -Xmx128m";

            var executor = new Executor();
            return executor.Execute(arguments, input, timeLimit, memoryLimit);
        }
    }
}
