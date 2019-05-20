using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.ViewModels.Test;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JudgeSystem.Services.Data
{
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

		public async Task<Test> GetById(int id)
		{
			return await repository.All()
				.FirstOrDefaultAsync(t => t.Id == id);
		}

		public IEnumerable<TestViewModel> TestsByProblem(int problemId)
		{
			return this.repository.All()
				.Where(t => t.ProblemId == problemId)
				.To<TestViewModel>()
				.ToList();
		}
	}
}
