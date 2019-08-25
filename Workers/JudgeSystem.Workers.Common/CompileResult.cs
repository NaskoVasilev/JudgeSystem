namespace JudgeSystem.Workers.Common
{
    public class CompileResult
	{
        public CompileResult()
        {
        }

		public CompileResult(string errors)
		{
			Errors = errors;
		}

		public string OutputFilePath { get; set; }

		public string Errors { get; private set; }

		public bool IsCompiledSuccessfully => string.IsNullOrEmpty(Errors);
	}
}
