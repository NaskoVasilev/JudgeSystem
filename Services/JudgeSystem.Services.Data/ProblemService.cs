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
	using JudgeSystem.Web.InputModels.Problem;

	using Microsoft.EntityFrameworkCore;
	using JudgeSystem.Web.ViewModels.Search;
    using System;
    using JudgeSystem.Common;

    public class ProblemService : IProblemService
	{
		private readonly IDeletableEntityRepository<Problem> problemRepository;

		public ProblemService(IDeletableEntityRepository<Problem> problemRepository)
		{
			this.problemRepository = problemRepository;
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
            if(!this.Exists(problem.Id))
            {
                throw new EntityNotFoundException();
            }

			problemRepository.Delete(problem);
			await problemRepository.SaveChangesAsync();
		}

		public async Task<Problem> GetById(int id)
		{
            if (!this.Exists(id))
            {
                throw new EntityNotFoundException();
            }

            return await problemRepository.All().FirstOrDefaultAsync(p => p.Id == id);
		}

		public async Task<Problem> GetByIdWithTests(int id)
		{
            if (!this.Exists(id))
            {
                throw new EntityNotFoundException();
            }

            return await problemRepository.All()
				.Include(p => p.Tests)
				.FirstOrDefaultAsync(p => p.Id == id);
		}

		public async Task<int> GetLessonId(int problemId)
		{
			Problem problem = await GetById(problemId);
			return problem.LessonId;
		}

		public int GetProblemMaxPoints(int id)
		{
            if (!this.Exists(id))
            {
                throw new EntityNotFoundException();
            }

            var problem = problemRepository.All().FirstOrDefault(p => p.Id == id);
			return problem.MaxPoints;
		}

        public string GetProblemName(int id)
        {
            string name = this.problemRepository.All()
                .Where(x => x.Id == id)
                .Select(x => x.Name)
                .FirstOrDefault();

            if(name == null)
            {
                throw new EntityNotFoundException("problem");
            }

            return name;
        }

        public IEnumerable<LessonProblemViewModel> LessonProblems(int lessonId)
		{
			return problemRepository.All()
				.Where(p => p.LessonId == lessonId)
				.To<LessonProblemViewModel>()
				.ToList();
		}

		public IEnumerable<SearchProblemViewModel> SearchByName(string keyword)
		{
            if(string.IsNullOrEmpty(keyword))
            {
                throw new ArgumentException(ErrorMessages.InvalidSearchKeyword);
            }

			keyword = keyword.ToLower();
			var results = problemRepository.All()
				.Where(p => p.Name.ToLower().Contains(keyword))
				.To<SearchProblemViewModel>()
				.ToList();

            return results;
		}

		public async Task<Problem> Update(ProblemEditInputModel model)
		{
            if (!this.Exists(model.Id))
            {
                throw new EntityNotFoundException();
            }

            Problem problem = await GetById(model.Id);
			if(problem == null)
			{
				throw new EntityNotFoundException(nameof(problem));
			}

			problem.Name = model.Name;
			problem.MaxPoints = model.MaxPoints;
			problem.IsExtraTask = model.IsExtraTask;
			problem.SubmissionType = model.SubmissionType;

            problemRepository.Update(problem);
			await problemRepository.SaveChangesAsync();
			return problem;
		}

        private bool Exists(int id)
        {
            return this.problemRepository.All().Any(x => x.Id == id);
        }
    }
}
