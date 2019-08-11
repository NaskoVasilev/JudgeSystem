namespace JudgeSystem.Services
{
    public interface IPasswordHashService
    {
        string HashPassword(string password);
    }
}
