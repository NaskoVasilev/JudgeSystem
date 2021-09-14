using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.Dtos.Test;
using JudgeSystem.Web.InputModels.Test;
using JudgeSystem.Web.InputModels.Problem;
using JudgeSystem.Common;

using Microsoft.EntityFrameworkCore;

namespace JudgeSystem.Services.Data
{
    public class TestService : ITestService
	{
		private readonly IRepository<Test> repository;
		private readonly IExecutedTestService executedTestService;

		public TestService(IRepository<Test> repository, IExecutedTestService executedTestService)
		{
			this.repository = repository;
			this.executedTestService = executedTestService;
		}

		public async Task<TestDto> Add(TestInputModel model)
		{
			Test test = model.To<Test>();
            test.OutputData = test.OutputData.Trim();
			await repository.AddAsync(test);
			return test.To<TestDto>();
		}

        public Task AddRange(IEnumerable<ProblemTestInputModel> tests, int problemId)
        {
            IEnumerable<Test> entites = tests.To<IEnumerable<Test>>();
            foreach (Test entity in entites)
            {
                entity.OutputData = entity.OutputData.Trim();
                entity.ProblemId = problemId;
            }
            return repository.AddRangeAsync(entites);
        }

        public async Task Delete(int id)
		{
            Test test = await repository.FindAsync(id);
			await executedTestService.DeleteExecutedTestsByTestId(test.Id);
            await repository.DeleteAsync(test);
		}

		public async Task<TDestination> GetById<TDestination>(int id)
		{
            TDestination test = await repository.All().Where(x => x.Id == id).To<TDestination>().FirstOrDefaultAsync();
            Validator.ThrowEntityNotFoundExceptionIfEntityIsNull(test, nameof(Test));
            return test;
		}

		public IEnumerable<T> GetTestsByProblemIdOrderedByIsTrialDescending<T>(int problemId)
		{
			var tests = repository.All()
                .Where(t => t.ProblemId == problemId)
                .OrderByDescending(t => t.IsTrialTest)
                .ThenBy(t => t.OrderBy)
                .To<T>()
				.ToList();
            return tests;
		}

		public async Task Update(TestEditInputModel model)
		{
            Test test = await repository.FindAsync(model.Id);
            test.IsTrialTest = model.IsTrialTest;
            test.InputData = model.InputData;
            test.OutputData = model.OutputData.Trim();
            await repository.UpdateAsync(test);
		}
    }
}
