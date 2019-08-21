using System.Collections.Generic;

namespace JudgeSystem.Workers.Common
{
	public class CompileResult
	{
		public CompileResult(ICollection<string> errors)
		{
			Errors = errors;
		}

		public CompileResult(string outputFile)
		{
			OutputFile = outputFile;
		}

		public string OutputFile { get; set; }

		public ICollection<string> Errors { get; set; }

		public bool IsCompiledSuccessfully => Errors == null || Errors.Count == 0;
	}
}
