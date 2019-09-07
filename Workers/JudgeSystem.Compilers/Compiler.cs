using System.Diagnostics;

using JudgeSystem.Workers.Common;

namespace JudgeSystem.Compilers
{
    internal class Compiler
    {
        public CompileResult Compile(string arguments)
        {
            var process = new Process();
            var info = new ProcessStartInfo
            {
                Arguments = arguments,
                FileName = CompilationSettings.ConsoleFile,
                UseShellExecute = false,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            process.StartInfo = info;

            process.Start();
            string errors = process.StandardError.ReadToEnd();
            process.WaitForExit();

            return new CompileResult(errors);
        }
    }
}
