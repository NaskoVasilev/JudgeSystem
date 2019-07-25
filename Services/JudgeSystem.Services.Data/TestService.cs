namespace JudgeSystem.Services.Data
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using JudgeSystem.Data.Common.Repositories;
	using JudgeSystem.Data.Models;
	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Web.Dtos.Test;
	using JudgeSystem.Web.ViewModels.Test;
	using JudgeSystem.Web.InputModels.Test;

	using Microsoft.EntityFrameworkCore;
    using JudgeSystem.Web.Infrastructure.Exceptions;

    public class TestService : ITestService
	{
		private readonly IRepository<Test> repository;
		private readonly IExecutedTestService executedTestService;

		public TestService(IRepository<Test> repository, IExecutedTestService executedTestService)
		{
			this.repository = repository;
			this.executedTestService = executedTestService;
		}

		public async Task<Test> Add(TestInputModel model)
		{
			Test test = model.To<TestInputModel, Test>();
			await repository.AddAsync(test);
			await repository.SaveChangesAsync();
			return test;
		}

		public async Task Delete(Test test)
		{
            if(!this.Exists(test.Id))
            {
                throw new EntityNotFoundException();
            }

			await executedTestService.DeleteExecutedTestsByTestId(test.Id);

			repository.Delete(test);
			await repository.SaveChangesAsync();
		}

		public async Task<Test> GetById(int id)
		{
			return await repository.All()
				.FirstOrDefaultAsync(t => t.Id == id);
		}

		public IEnumerable<TestDataDto> GetTestsByProblemId(int problemId)
		{
			var tests = repository.All()
				.Where(t => t.ProblemId == problemId)
				.To<TestDataDto>()
				.ToList();
			return tests;
		}

		public IEnumerable<TestViewModel> GetTestsByProblemIdOrderedByIsTrialDescending(int problemId)
		{
			return this.repository.All()
				.Where(t => t.ProblemId == problemId)
				.OrderByDescending(t => t.IsTrialTest)
				.To<TestViewModel>()
				.ToList();
		}

		public async Task Update(Test test)
		{
            if(!this.Exists(test.Id))
            {
                throw new EntityNotFoundException(nameof(test));
            }

			repository.Update(test);
			await repository.SaveChangesAsync();
		}

        private bool Exists(int id)
        {
            return this.repository.All().Any(x => x.Id == id);
        }
	}
}
