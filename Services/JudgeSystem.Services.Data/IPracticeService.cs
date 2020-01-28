using System.Threading.Tasks;

using JudgeSystem.Web.Dtos.Lesson;
using JudgeSystem.Web.ViewModels.Practice;

namespace JudgeSystem.Services.Data
{
    public interface IPracticeService
    {
        Task<int> Create(int lessonId);

        Task AddUserToPracticeIfNotAdded(string userId, int value);

        Task<LessonDto> GetLesson(int practiceId);

        PracticeAllResultsViewModel GetPracticeResults(int id, int page, int entitesPerPage);

        int GetPracticeResultsPagesCount(int id, int entitesPerPage);
    }
}
