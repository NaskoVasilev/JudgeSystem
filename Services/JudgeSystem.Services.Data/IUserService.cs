using System.Collections.Generic;
using System.Threading.Tasks;
using JudgeSystem.Web.ViewModels.User;

namespace JudgeSystem.Services.Data
{
    public interface IUserService
	{
		List<UserCompeteResultViewModel> GetContestResults(string userId);

		List<UserPracticeResultViewModel> GetPracticeResults(string userId);

		IEnumerable<UserCompeteResultViewModel> GetUserExamResults(string userId);

        Task DeleteUserData(string userId, string studentId);

        Task<UserNamesViewModel> GetUserNames(string userId);

        IEnumerable<UserViewModel> All();

        public bool IsExistingUserWithNotConfirmedEmail(string username);
    }
}
