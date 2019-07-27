namespace JudgeSystem.Web.ViewModels.User
{
	using System.Collections.Generic;

	public class UserResultsViewModel
	{
		public List<UserCompeteResultViewModel> ContestResults { get; set; }

		public List<UserPracticeResultViewModel> PracticeResults { get; set; }

        public string UserId { get; set; }
    }
}
