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

        public async Task Create(ExecutedTest executedTest) => await repository.AddAsync(executedTest);

        public async Task DeleteExecutedTestsByTestId(int testId)
		{
            var entities = repository.All().Where(t => t.TestId == testId).ToList();
            await repository.DeleteRangeAsync(entities);
		}
	}
}
