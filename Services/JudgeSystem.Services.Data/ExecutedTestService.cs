using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;

namespace JudgeSystem.Services.Data
{
	public class ExecutedTestService : IExecutedTestService
	{
		private readonly IRepository<ExecutedTest> repository;

		public ExecutedTestService(IRepository<ExecutedTest> repository)
		{
			this.repository = repository;
		}

		public async Task Create(ExecutedTest executedTest)
		{
			await repository.AddAsync(executedTest);
			await repository.SaveChangesAsync();
		}

		public async Task DeleteExecutedTestsByTestId(int testId)
		{
			foreach (var executedTest in repository.All().Where(t => t.TestId == testId).ToList())
			{
				repository.Delete(executedTest);
			}

			await repository.SaveChangesAsync();
		}
	}
}
