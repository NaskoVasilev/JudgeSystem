using JudgeSystem.Data.Models;
using System.Threading.Tasks;

namespace JudgeSystem.Services.Data
{
    public interface IPracticeService
    {
        Task<int> Create(int lessonId);

        Task AddUserToPracticeIfNotAdded(string userId, int value);

        int GetLessonId(int practiceId);
    }
}
