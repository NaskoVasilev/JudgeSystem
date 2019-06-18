namespace JudgeSystem.Services.Data
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	using JudgeSystem.Data.Models;
	using JudgeSystem.Web.ViewModels.Contest;

	public interface IContestService
	{
		Task Create(Contest contest);

		Task<bool> AddUserToContestIfNotAdded(string userId, int contestId);

		IEnumerable<ActiveContestViewModel> GetActiveContests();

		IEnumerable<PreviousContestViewModel> GetPreviousContests(int passedDays);
	}
}
