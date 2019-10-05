using System.Threading.Tasks;

using JudgeSystem.Web.Dtos.Lesson;

namespace JudgeSystem.Services.Data
{
    public interface IPracticeService
    {
        Task<int> Create(int lessonId);

        Task AddUserToPracticeIfNotAdded(string userId, int value);

        Task<LessonDto> GetLesson(int practiceId);
    }
}
