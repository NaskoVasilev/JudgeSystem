using JudgeSystem.Workers.Common;
using System;
using System.Diagnostics;

namespace JudgeSystem.Executors
{
	public class CSharpExecutor
	{
		public ExecutionResult ProcessExecutionResult(string dllFilePath, string input)
		{
			string commandPromptArgument = @"/C dotnet " + dllFilePath;

			try
			{
				using (Process process = new Process())
				{
					process.StartInfo.FileName = "cmd.exe";
					process.StartInfo.Arguments = commandPromptArgument;
					process.StartInfo.RedirectStandardInput = true;
					process.StartInfo.RedirectStandardOutput = true;
					process.StartInfo.CreateNoWindow = true;
					process.StartInfo.UseShellExecute = false;

					process.Start();
					process.StandardInput.WriteLine(input);
					process.StandardInput.Flush();
					process.StandardInput.Close();
					process.WaitForExit();

					string output = process.StandardOutput.ReadToEnd();
					return new ExecutionResult { Output = output };
				}
			}
			catch (Exception ex)
			{
				return new ExecutionResult { Error = ex.Message };
			}
		}
	}
}
