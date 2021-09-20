namespace JudgeSystem.Services
{
    public interface IValidationService
    {
        bool IsValidFileExtension(string fileName);

        bool IsValidFileExtension(string fileName, params string[] extensions);
    }
}
