using System.Collections.Generic;
using System.Threading.Tasks;

using JudgeSystem.Web.InputModels.Contest;
using JudgeSystem.Web.ViewModels.Contest;

namespace JudgeSystem.Services.Data
{
	public interface IContestService
	{
		Task Create(ContestCreateInputModel contestCreateInputModel);

		Task<bool> AddUserToContestIfNotAdded(string userId, int contestId);

		IEnumerable<ActiveContestViewModel> GetActiveContests();

		IEnumerable<PreviousContestViewModel> GetPreviousContests(int passedDays);

		Task<T> GetById<T>(int contestId);

		IEnumerable<ContestBreifInfoViewModel> GetActiveAndFollowingContests();

		Task Update(ContestEditInputModel model);

		Task Delete(int id);

		IEnumerable<ContestViewModel> GetAllConests(int page);

		int GetNumberOfPages();

		ContestAllResultsViewModel GetContestReults(int contestId, int page, int entitiesPerPage);

		int GetContestResultsPagesCount(int contestId, int entitiesPerPage);

        Task<int> GetLessonId(int contestId);

        Task<ContestSubmissionsViewModel> GetContestSubmissions(int contestId, string userId, int? problemId, int page, string baseUrl);
        
        bool IsActive(int contestId);
        
        Task AddAllowedIpAddress(ContestAllowedIpAddressesInputModel model, int id);
        
        Task RemoveAllowedIpAddress(int contestId, int ipAddressId);
    }
}
