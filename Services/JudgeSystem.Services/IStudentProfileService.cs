using System.Threading.Tasks;

namespace JudgeSystem.Services
{
    public interface IStudentProfileService
    {
        Task<string> SendActivationEmail(string email);
    }
}
