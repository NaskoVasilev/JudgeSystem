using System.Collections.Generic;
using JudgeSystem.Web.ViewModels.User;

namespace JudgeSystem.Services.Data
{
	public interface IUserService
	{
		List<UserCompeteResultViewModel> GetContestResults(string userId);

		List<UserPracticeResultViewModel> GetPracticetResults(string userId);

		IEnumerable<UserCompeteResultViewModel> GetUserExamResults(string userId);
    }
}
