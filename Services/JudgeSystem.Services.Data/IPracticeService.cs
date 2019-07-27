using JudgeSystem.Data.Models;
using System.Threading.Tasks;

namespace JudgeSystem.Services.Data
{
    public interface IPracticeService
    {
        Task<Practice> Create(int lessonId);

        Task AddUserToPracticeIfNotAdded(string userId, int value);
    }
}
