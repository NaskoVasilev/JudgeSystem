using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using JudgeSystem.Common.Extensions;
using JudgeSystem.Workers.Common;

namespace JudgeSystem.Executors
{
    internal class Executor
    {
        private const int TimeIntervalBetweenTwoMemoryConsumptionRequests = 45;
        private const double TimeLimitMultiplier = 1.5;
        // We need to wait the proccess to exit at least 2500 ms beacuse if there is some runtime error the proccess working between 2000 and 2500 ms
        private const int MinWorkingTimeForProcessInMilliseconds = 2500;
        private const string ReadingDataFromConsoleIsRequired = "You should read some data from console in your application.";

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

                string error = string.Empty;
                if (!string.IsNullOrEmpty(input))
                {
                    try
                    {
                        await process.StandardInput.WriteLineAsync(input);
                        await process.StandardInput.FlushAsync();
                        process.StandardInput.Close();
                    }
                    catch (IOException)
                    {
                        error = ReadingDataFromConsoleIsRequired;
                    }
                }

                int timeout = GetTimeout(timeLimit);
                bool exited = process.WaitForExit(timeout);

                if (!exited)
                {
                    if (!process.HasExited)
                    {
                        process.KillTree();
                    }

                    memoryTaskCancellationToken.Cancel();
                    executionResult.Type = ProcessExecutionResultType.TimeLimit;
                    executionResult.TimeWorked = TimeSpan.FromMilliseconds(timeout);
                    return executionResult;
                }

                // Close the memory consumption check thread
                memoryTaskCancellationToken.Cancel();

                string output = await process.StandardOutput.ReadToEndAsync();
                error += await process.StandardError.ReadToEndAsync();

                executionResult.Error = error;
                executionResult.Output = output.Trim();
                executionResult.ExitCode = process.ExitCode;
                executionResult.TimeWorked = process.ExitTime - process.StartTime;
                executionResult.PrivilegedProcessorTime = process.PrivilegedProcessorTime;
                executionResult.UserProcessorTime = process.UserProcessorTime;

                //We need to check first if there is runtime error because it is with more priority
                if (!string.IsNullOrEmpty(executionResult.Error))
                {
                    executionResult.Type = ProcessExecutionResultType.RunTimeError;
                }
                else if (executionResult.TimeWorked.TotalMilliseconds > timeLimit)
                {
                    executionResult.Type = ProcessExecutionResultType.TimeLimit;
                }
                else if (executionResult.MemoryUsed > memoryLimit)
                {
                    executionResult.Type = ProcessExecutionResultType.MemoryLimit;
                }

                return executionResult;
            }
        }

        private int GetTimeout(int timeLimit)
        {
            int timeout = (int)(timeLimit * TimeLimitMultiplier);

            if(timeout < MinWorkingTimeForProcessInMilliseconds)
            {
                timeout = MinWorkingTimeForProcessInMilliseconds;
            }

            return timeout;
        }
    }
}
