namespace JudgeSystem.Services.Data
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using JudgeSystem.Data.Common.Repositories;
	using JudgeSystem.Data.Models;
	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Web.ViewModels.Test;

	using Microsoft.EntityFrameworkCore;

	public class TestService : ITestService
	{
		private readonly IRepository<Test> repository;

		public TestService(IRepository<Test> repository)
		{
			this.repository = repository;
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

		public IEnumerable<TestViewModel> TestsByProblem(int problemId)
		{
			return this.repository.All()
				.Where(t => t.ProblemId == problemId)
				.OrderByDescending(t => t.IsTrialTest)
				.To<TestViewModel>()
				.ToList();
		}

		public async Task Update(Test test)
		{
			repository.Update(test);
			await repository.SaveChangesAsync();
		}
	}
}
