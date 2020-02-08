using JudgeSystem.Web.ViewModels.Lesson;
using JudgeSystem.Web.ViewModels.Student;

namespace JudgeSystem.Web.ViewModels.Contest
{
    public class ContestResultViewModel : LessonResultViewModel
	{
		public StudentBreifInfoViewModel Student { get; set; }

        public string UserId { get; set; }
	}
}
