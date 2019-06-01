namespace JudgeSystem.Services.Data
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using JudgeSystem.Data.Common.Repositories;
	using JudgeSystem.Data.Models;
	using Services.Mapping;
	using JudgeSystem.Web.Infrastructure.Exceptions;
	using JudgeSystem.Web.ViewModels.Problem;

	using Microsoft.EntityFrameworkCore;

	public class ProblemService : IProblemService
	{
		private readonly IRepository<Problem> problemRepository;
		private readonly IRepository<Test> testRepository;

		public ProblemService(IRepository<Problem> problemRepository, IRepository<Test> testRepository)
		{
			this.problemRepository = problemRepository;
			this.testRepository = testRepository;
		}

		public async Task<Problem> Create(ProblemInputModel model)
		{
			Problem problem = model.To<ProblemInputModel, Problem>();
			await problemRepository.AddAsync(problem);
			await problemRepository.SaveChangesAsync();
			return problem;
		}

		public async Task Delete(Problem problem)
		{
			IEnumerable<Test> problemTests = testRepository
				.All()
				.Where(t => t.ProblemId == problem.Id)
				.ToList();

			foreach (var test in problemTests)
			{
				testRepository.Delete(test);
			}

			problemRepository.Delete(problem);
			await problemRepository.SaveChangesAsync();
		}

		public async Task<Problem> GetById(int id)
		{
			return await problemRepository.All().FirstOrDefaultAsync(p => p.Id == id);
		}

		public async Task<Problem> GetByIdWithTests(int id)
		{
			return await problemRepository.All()
				.Include(p => p.Tests)
				.FirstOrDefaultAsync(p => p.Id == id);
		}

		public IEnumerable<LessonProblemViewModel> LesosnProblems(int lessonId)
		{
			return problemRepository.All()
				.Where(p => p.LessonId == lessonId)
				.To<LessonProblemViewModel>()
				.ToList();
		}

		public async Task<Problem> Update(ProblemEditInputModel model)
		{
			Problem problem = await GetById(model.Id);
			if(problem == null)
			{
				throw new EntityNullException(nameof(problem));
			}

			problem.Name = model.Name;
			problem.MaxPoints = model.MaxPoints;
			problem.IsExtraTask = model.IsExtraTask;

			problemRepository.Update(problem);
			await problemRepository.SaveChangesAsync();
			return problem;
		}
	}
}
