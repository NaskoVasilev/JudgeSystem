using System.Collections.Generic;
using System.Threading.Tasks;

using JudgeSystem.Services.Models.Users;
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

        bool IsExistingUserWithNotConfirmedEmail(string username);

        Task ImportAsync(IEnumerable<UserImportServiceModel> users);
    }
}
