using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.ViewModels.Problem;
using JudgeSystem.Web.InputModels.Problem;

using JudgeSystem.Web.ViewModels.Search;
using JudgeSystem.Common;
using JudgeSystem.Web.Dtos.Problem;
using JudgeSystem.Common.Exceptions;

using Microsoft.EntityFrameworkCore;

namespace JudgeSystem.Services.Data
{
    public class ProblemService : IProblemService
	{
		private readonly IDeletableEntityRepository<Problem> problemRepository;

		public ProblemService(IDeletableEntityRepository<Problem> problemRepository)
		{
			this.problemRepository = problemRepository;
		}

		public async Task<ProblemDto> Create(ProblemInputModel model)
		{
			Problem problem = model.To<Problem>();
			await problemRepository.AddAsync(problem);
			await problemRepository.SaveChangesAsync();
			return problem.To<ProblemDto>();
		}

		public async Task<ProblemDto> Delete(int id)
		{
            var problem = await problemRepository.FindAsync(id);

			problemRepository.Delete(problem);
			await problemRepository.SaveChangesAsync();

            return problem.To<ProblemDto>();
		}

		public async Task<TDestination> GetById<TDestination>(int id)
		{    
            var problem = await problemRepository.All().Where(x => x.Id == id).To<TDestination>().FirstOrDefaultAsync();
            if (problem == null)
            {
                throw new EntityNotFoundException(nameof(problem));
            }
            return problem;
        }

		public async Task<int> GetLessonId(int problemId)
		{
            Problem problem = await problemRepository.FindAsync(problemId);
			return problem.LessonId;
		}

        public ProblemConstraintsDto GetProblemConstraints(int id)
        {
            var problem = problemRepository.All()
                .Where(x => x.Id == id)
                .To<ProblemConstraintsDto>()
                .FirstOrDefault();

            if(problem == null)
            {
                throw new EntityNotFoundException(nameof(problem));
            }
            return problem;
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
                throw new BadRequestException(ErrorMessages.InvalidSearchKeyword);
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
            var problem = await problemRepository.FindAsync(model.Id);

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
    }
}
