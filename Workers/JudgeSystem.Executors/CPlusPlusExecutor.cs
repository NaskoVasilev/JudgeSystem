﻿using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Workers.Common;

namespace JudgeSystem.Executors
{
    public class CPlusPlusExecutor : IExecutor
    {
        public Task<ExecutionResult> Execute(string filePath, string input, int timeLimit, int memoryLimit)
        {
            string arguments = filePath;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                arguments = $"{GlobalConstants.ConsoleComamndPrefix} {CompilationSettings.SetCPlusPlusCompilerPathCommand} & {arguments}";
            }

            var executor = new Executor();
            return executor.Execute(arguments, input, timeLimit, memoryLimit);
        }
    }
}
