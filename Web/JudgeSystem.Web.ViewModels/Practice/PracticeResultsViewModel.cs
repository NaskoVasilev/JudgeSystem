using JudgeSystem.Web.ViewModels.Lesson;

namespace JudgeSystem.Web.ViewModels.Practice
{
    public class PracticeResultViewModel : LessonResultViewModel
    {
        public string UserId { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }
    }
}
