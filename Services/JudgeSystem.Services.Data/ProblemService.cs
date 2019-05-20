using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.ViewModels.Problem;
using Microsoft.EntityFrameworkCore;

namespace JudgeSystem.Services.Data
{
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
	}
}
