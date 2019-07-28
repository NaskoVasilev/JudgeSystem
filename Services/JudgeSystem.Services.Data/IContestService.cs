namespace JudgeSystem.Services.Data
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	using JudgeSystem.Data.Models;
	using JudgeSystem.Web.InputModels.Contest;
	using JudgeSystem.Web.ViewModels.Contest;

	public interface IContestService
	{
		Task Create(Contest contest);

		Task<bool> AddUserToContestIfNotAdded(string userId, int contestId);

		IEnumerable<ActiveContestViewModel> GetActiveContests();

		IEnumerable<PreviousContestViewModel> GetPreviousContests(int passedDays);

		Task<T> GetById<T>(int contestId);

		IEnumerable<ContestBreifInfoViewModel> GetActiveAndFollowingContests();

		Task UpdateContest(ContestEditInputModel model);

		Task DeleteContestById(int id);

		IEnumerable<ContestViewModel> GetAllConests(int page);

		int GetNumberOfPages();

		ContestAllResultsViewModel GetContestReults(int contestId, int page);

		int GetContestResultsPagesCount(int contestId);

        int GetFirstProblemId(int contestId);

        int GetLessonId(int contestId);
    }
}
