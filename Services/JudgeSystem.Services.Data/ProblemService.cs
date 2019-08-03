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
    using JudgeSystem.Web.Dtos.Problem;

    public class ProblemService : IProblemService
	{
		private readonly IDeletableEntityRepository<Problem> problemRepository;

		public ProblemService(IDeletableEntityRepository<Problem> problemRepository)
		{
			this.problemRepository = problemRepository;
		}

		public async Task<ProblemDto> Create(ProblemInputModel model)
		{
			Problem problem = model.To<ProblemInputModel, Problem>();
			await problemRepository.AddAsync(problem);
			await problemRepository.SaveChangesAsync();
			return problem.To<ProblemDto>();
		}

		public async Task<ProblemDto> Delete(int id)
		{
            var problem = await problemRepository.All().FirstOrDefaultAsync(x => x.Id == id);
            if(problem == null)
            {
                throw new EntityNotFoundException();
            }

			problemRepository.Delete(problem);
			await problemRepository.SaveChangesAsync();

            return problem.To<ProblemDto>();
		}

		public async Task<TDestination> GetById<TDestination>(int id)
		{
            if (!this.Exists(id))
            {
                throw new EntityNotFoundException();
            }

            return await problemRepository.All().Where(x => x.Id == id).To<TDestination>().FirstOrDefaultAsync();
		}

		public async Task<int> GetLessonId(int problemId)
		{
			Problem problem = await problemRepository.All().FirstOrDefaultAsync(x => x.Id == problemId);
            if(problem == null)
            {
                throw new EntityNotFoundException();
            }
			return problem.LessonId;
		}

        public ProblemConstraintsDto GetProblemConstraints(int id)
        {
            if(!this.Exists(id))
            {
                throw new EntityNotFoundException("problem");
            }

            return problemRepository.All()
                .Where(x => x.Id == id)
                .To<ProblemConstraintsDto>()
                .First();
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

		public async Task<ProblemDto> Update(ProblemEditInputModel model)
		{
            var problem = await problemRepository.All().FirstOrDefaultAsync(x => x.Id == model.Id);
            if (problem == null)
            {
                throw new EntityNotFoundException();
            }

			problem.Name = model.Name;
			problem.MaxPoints = model.MaxPoints;
			problem.IsExtraTask = model.IsExtraTask;
			problem.SubmissionType = model.SubmissionType;
            problem.AllowedTimeInMilliseconds = model.AllowedTimeInMilliseconds;
            problem.AllowedMemoryInMegaBytes = model.AllowedMemoryInMegaBytes;
            problemRepository.Update(problem);
			await problemRepository.SaveChangesAsync();

            return problem.To<ProblemDto>();
		}

        private bool Exists(int id)
        {
            return this.problemRepository.All().Any(x => x.Id == id);
        }
    }
}
