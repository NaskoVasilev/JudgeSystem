using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Common.Extensions;
using JudgeSystem.Workers.Common;

namespace JudgeSystem.Executors
{
    internal class Executor
    {
        private const int TimeIntervalBetweenTwoMemoryConsumptionRequests = 45;
        private const string ReadingDataFromConsoleIsRequired = "You should read some data from console in your application.";

        public async Task<ExecutionResult> Execute(string arguments, string input, int timeLimit, int memoryLimit)
        {
            var executionResult = new ExecutionResult();

            using (var process = new Process())
            {
                if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    process.StartInfo.FileName = GlobalConstants.ConsoleFile;
                    process.StartInfo.Arguments = arguments;
                }
                else
                {
                    process.StartInfo.FileName = arguments;
                }

                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;

                var stopwatch = Stopwatch.StartNew();
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
                        using (process.StandardInput)
                        {
                            await process.StandardInput.WriteLineAsync(input);
                            await process.StandardInput.FlushAsync();
                        }
                    }
                    catch (IOException ex)
                    {
                        error = ReadingDataFromConsoleIsRequired;
                    }
                }

                bool exited = process.WaitForExit(timeLimit);
                long timeInMillisiconds = stopwatch.ElapsedMilliseconds;

                if (!exited)
                {
                    if (!process.HasExited)
                    {
                        process.KillTree();
                    }
                }

                // Close the memory consumption check thread
                memoryTaskCancellationToken.Cancel();

                string output = await process.StandardOutput.ReadToEndAsync();
                error += await process.StandardError.ReadToEndAsync();

                executionResult.Error = error;
                executionResult.Output = output.Trim();
                executionResult.ExitCode = process.ExitCode;
                executionResult.TimeWorked = TimeSpan.FromMilliseconds(timeInMillisiconds);

                //We need to check first if there is runtime error because it is with more priority
                if (!string.IsNullOrEmpty(executionResult.Error))
                {
                    executionResult.Type = ProcessExecutionResultType.RunTimeError;
                }
                else if (executionResult.TimeWorked.TotalMilliseconds >= timeLimit)
                {
                    executionResult.Type = ProcessExecutionResultType.TimeLimit;
                }
                else if (executionResult.MemoryUsed >= memoryLimit)
                {
                    executionResult.Type = ProcessExecutionResultType.MemoryLimit;
                }

                return executionResult;
            }
        }
    }
}
