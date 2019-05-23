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
		private readonly IRepository<Problem> repository;

		public ProblemService(IRepository<Problem> repository)
		{
			this.repository = repository;
		}

		public async Task<Problem> Create(ProblemInputModel model)
		{
			Problem problem = model.To<ProblemInputModel, Problem>();
			await repository.AddAsync(problem);
			await repository.SaveChangesAsync();
			return problem;
		}

		public async Task Delete(Problem problem)
		{
			repository.Delete(problem);
			await repository.SaveChangesAsync();
		}

		public async Task<Problem> GetById(int id)
		{
			return await repository.All().FirstOrDefaultAsync(p => p.Id == id);
		}

		public IEnumerable<LessonProblemViewModel> LesosnProblems(int lessonId)
		{
			return repository.All()
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

			repository.Update(problem);
			await repository.SaveChangesAsync();
			return problem;
		}
	}
}
