using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.Dtos.Test;
using JudgeSystem.Web.ViewModels.Test;
using JudgeSystem.Web.InputModels.Test;
using JudgeSystem.Common.Exceptions;

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
			await repository.AddAsync(test);
			await repository.SaveChangesAsync();
			return test.To<TestDto>();
		}

		public async Task Delete(int id)
		{
            var test = await repository.FindAsync(id);
			await executedTestService.DeleteExecutedTestsByTestId(test.Id);
            repository.Delete(test);
			await repository.SaveChangesAsync();
		}

		public async Task<TDestination> GetById<TDestination>(int id)
		{
            var test = await repository.All().Where(x => x.Id == id).To<TDestination>().FirstOrDefaultAsync();
            if(test == null)
            {
                throw new EntityNotFoundException();
            }
            return test;
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

		public async Task Update(TestEditInputModel model)
		{
            var test = await repository.FindAsync(model.Id);
            test.InputData = model.InputData.Trim();
            test.OutputData = model.OutputData.Trim();
            repository.Update(test);
			await repository.SaveChangesAsync();
		}
	}
}
