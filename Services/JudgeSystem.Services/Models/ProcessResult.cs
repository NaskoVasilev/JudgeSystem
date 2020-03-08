namespace JudgeSystem.Services.Models
{
    public class ProcessResult
    {
        public ProcessResult(string output, string errors)
        {
            Output = output;
            Errors = errors;
        }

        public string Output { get; set; }

        public string Errors { get; private set; }

        public bool IsSuccessfull => string.IsNullOrEmpty(Errors);
    }
}
