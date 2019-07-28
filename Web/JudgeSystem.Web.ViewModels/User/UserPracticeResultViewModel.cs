namespace JudgeSystem.Web.ViewModels.User
{
	public class UserPracticeResultViewModel
	{
        public int LessonId { get; set; }

        public int PracticeId { get; set; }

        public string LessonName { get; set; }

		public int MaxPoints { get; set; }

		public int ActualPoints { get; set; }
	}
}
