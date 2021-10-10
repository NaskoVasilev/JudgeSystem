namespace JudgeSystem.Services.Validations.Contracts
{
    public interface IEmailValidationService
    {
        bool IsValid(string email);
    }
}
