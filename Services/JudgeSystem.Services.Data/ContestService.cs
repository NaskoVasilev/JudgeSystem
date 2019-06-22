namespace JudgeSystem.Services.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using JudgeSystem.Data.Common.Repositories;
	using JudgeSystem.Data.Models;
	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Web.ViewModels.Contest;
    using Microsoft.EntityFrameworkCore;

    public class ContestService : IContestService
	{
		private readonly IDeletableEntityRepository<Contest> repository;
		private readonly IRepository<UserContest> userContestRepository;

		public ContestService(IDeletableEntityRepository<Contest> repository, IRepository<UserContest> userContestRepository)
		{
			this.repository = repository;
			this.userContestRepository = userContestRepository;
		}

		public async Task<bool> AddUserToContestIfNotAdded(string userId, int contestId)
		{
			if(this.userContestRepository.All().SingleOrDefault(uc => uc.UserId == userId && uc.ContestId == contestId) != null)
			{
				return false;
			}

			await userContestRepository.AddAsync(new UserContest { UserId = userId, ContestId = contestId });
			await userContestRepository.SaveChangesAsync();
			return true;
		}

		public async Task Create(Contest contest)
		{
			await repository.AddAsync(contest);
			await repository.SaveChangesAsync();
		}

		public IEnumerable<ActiveContestViewModel> GetActiveContests()
		{
			var contests = repository.All()
				.Where(c => c.IsActive)
				.To<ActiveContestViewModel>()
				.ToList();
			return contests;
		}

		public async Task<T> GetById<T>(int contestId)
		{
			var contest = await repository.All().FirstOrDefaultAsync(c => c.Id == contestId);
			return contest.To<T>();
		}

		public IEnumerable<ContestBreifInfoViewModel> GetActiveAndFollowingContests()
		{
			var followingContests = this.repository.All()
				.Where(c => c.EndTime > DateTime.Now)
				.To<ContestBreifInfoViewModel>()
				.ToList();

			return followingContests;
		}

		public IEnumerable<PreviousContestViewModel> GetPreviousContests(int passedDays)
		{
			var contests = repository.All()
				.Where(c => c.EndTime < DateTime.Now && (DateTime.Now - c.EndTime).Days <= passedDays)
				.To<PreviousContestViewModel>().ToList();
			return contests;
		}
	}
}
