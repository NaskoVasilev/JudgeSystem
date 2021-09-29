using System.Diagnostics;

using JudgeSystem.Common;
using JudgeSystem.Workers.Common;

namespace JudgeSystem.Compilers
{
    internal class Compiler
    {
        public CompileResult Compile(string arguments, string processFileName)
        {
            var process = new Process();
            var info = new ProcessStartInfo
            {
                Arguments = arguments,
                FileName = processFileName,
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
