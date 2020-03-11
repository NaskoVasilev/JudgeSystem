using System.Diagnostics;
using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Common.Extensions;
using JudgeSystem.Services.Models;

namespace JudgeSystem.Services
{
    public class ProcessRunner : IProcessRunner
    {
        public const int DefaultTimeout = 300;

        public const int BuildCommandTimeout = 1000;

        public const string DotnetListTestsCommand = "dotnet test --list-tests";

        public const string DotnetBuildProjectCommand = "dotnet build";

        public const string DotnetRunTestCommand = "dotnet test --filter FullyQualifiedName={0}";

        public const string DotnetRunTestsCommand = "dotnet test";

        public const string BuildFaildMessage = "Build failed";

        public async Task<ProcessResult> Run(string arguments, string woringDirectory, int timeout = DefaultTimeout)
        {

            using var process = new Process();
            var info = new ProcessStartInfo
            {
                Arguments = $"{GlobalConstants.ConsoleComamndPrefix} {arguments}",
                FileName = GlobalConstants.ConsoleFile,
                WorkingDirectory = woringDirectory,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                UseShellExecute = false,
            };

            process.StartInfo = info;
            process.Start();
            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();
            
            bool exited = process.WaitForExit(timeout);
            if (!exited)
            {
                process.KillTree();
            }

            return new ProcessResult(output, error);
        }

        public string PrependChangeDirectoryCommand(string command, string directoryPath) =>
            $"cd /d {directoryPath} & {command}";
    }
}
