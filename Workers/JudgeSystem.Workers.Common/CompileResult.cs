using System.Collections.Generic;

namespace JudgeSystem.Workers.Common
{
	public class CompileResult
	{
		public CompileResult(ICollection<string> errors)
		{
			this.Errors = errors;
		}

		public CompileResult(string outputFile)
		{
			OutputFile = outputFile;
		}

		public string OutputFile { get; set; }

		public ICollection<string> Errors { get; set; }

		public bool IsCompiledSuccessfully => this.Errors == null || this.Errors.Count == 0;
	}
}
