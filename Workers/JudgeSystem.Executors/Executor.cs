using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using JudgeSystem.Workers.Common;

namespace JudgeSystem.Executors
{
    internal class Executor
    {
        private const int ProcessMaxRunningTime = 1000;

        public async Task<ExecutionResult> Execute(string arguments, string input, int timeLimit, int memoryLimit)
        {
            var executionResult = new ExecutionResult();

            using (var process = new Process())
            {
                process.StartInfo.FileName = CompilationSettings.ConsoleFile;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;

                process.Start();

                const int TimeIntervalBetweenTwoMemoryConsumptionRequests = 45;
                var memoryTaskCancellationToken = new CancellationTokenSource();
                var memoryTask = Task.Run(
                    () =>
                    {
                        while (true)
                        {
                            try
                            {
                                if (process.HasExited)
                                {
                                    return;
                                }
                                executionResult.MemoryUsed = Math.Max(executionResult.MemoryUsed, process.PeakWorkingSet64);
                            }
                            catch (InvalidOperationException)
                            {
                                return;
                            }

                            if (memoryTaskCancellationToken.IsCancellationRequested)
                            {
                                return;
                            }

                            Thread.Sleep(TimeIntervalBetweenTwoMemoryConsumptionRequests);
                        }
                    },
                    memoryTaskCancellationToken.Token);

                if (!string.IsNullOrEmpty(input))
                {
                    await process.StandardInput.WriteLineAsync(input);
                    await process.StandardInput.FlushAsync();
                    process.StandardInput.Close();
                }

                bool exited = process.WaitForExit(ProcessMaxRunningTime);
                if (!exited)
                {
                    if (!process.HasExited)
                    {
                        process.Kill();
                    }
                    executionResult.Type = ProcessExecutionResultType.TimeLimit;
                }

                // Close the memory consumption check thread
                memoryTaskCancellationToken.Cancel();

                string output = await process.StandardOutput.ReadToEndAsync();
                string error = await process.StandardError.ReadToEndAsync();

                executionResult.Error = error;
                executionResult.Output = output.Trim();
                executionResult.ExitCode = process.ExitCode;
                executionResult.TimeWorked = process.ExitTime - process.StartTime;
                executionResult.PrivilegedProcessorTime = process.PrivilegedProcessorTime;
                executionResult.UserProcessorTime = process.UserProcessorTime;

                if (executionResult.TotalProcessorTime.TotalMilliseconds > timeLimit)
                {
                    executionResult.Type = ProcessExecutionResultType.TimeLimit;
                }

                if (!string.IsNullOrEmpty(executionResult.Error))
                {
                    executionResult.Type = ProcessExecutionResultType.RunTimeError;
                }

                if (executionResult.MemoryUsed > memoryLimit)
                {
                    executionResult.Type = ProcessExecutionResultType.MemoryLimit;
                }

                return executionResult;
            }
        }
    }
}
