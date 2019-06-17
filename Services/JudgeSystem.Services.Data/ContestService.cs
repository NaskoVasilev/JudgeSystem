namespace JudgeSystem.Services.Data
{
	using System;
	using System.Threading.Tasks;
	using JudgeSystem.Data.Common.Repositories;
	using JudgeSystem.Data.Models;

	public class ContestService : IContestService
	{
		private readonly IDeletableEntityRepository<Contest> repository;

		public ContestService(IDeletableEntityRepository<Contest> repository)
		{
			this.repository = repository;
		}

		public async Task Create(Contest contest)
		{
			await repository.AddAsync(contest);
			await repository.SaveChangesAsync();
		}
	}
}
