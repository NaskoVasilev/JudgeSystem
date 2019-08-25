using System.IO;
using System.Threading.Tasks;

using JudgeSystem.Workers.Common;

namespace JudgeSystem.Executors
{
    public class JavaExecutor : IExecutor
    {
        public Task<ExecutionResult> Execute(string filePath, string input, int timeLimit, int memoryLimit)
        {
            string workingDirectory = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileName(filePath);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string arguments = $"/c cd {workingDirectory} & set PATH=%PATH%;{CompilationSettings.JavaCompilerPath}; & java -Xmx128m -jar -Djava.security.manager -Djava.security.policy=={fileNameWithoutExtension}.policy {fileName}";

            var executor = new Executor();
            return executor.Execute(arguments, input, timeLimit, memoryLimit);
        }
    }
}
