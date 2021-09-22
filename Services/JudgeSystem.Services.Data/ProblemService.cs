﻿using System.Linq;
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
			return problem.To<ProblemDto>();
		}

		public async Task<ProblemDto> Delete(int id)
		{
            Problem problem = await problemRepository.FindAsync(id);
			await problemRepository.DeleteAsync(problem);
            return problem.To<ProblemDto>();
		}

		public async Task<TDestination> GetById<TDestination>(int id)
		{    
            TDestination problem = await problemRepository
                .All()
                .Where(x => x.Id == id)
                .To<TDestination>()
                .FirstOrDefaultAsync();
            
            Validator.ThrowEntityNotFoundExceptionIfEntityIsNull(problem, nameof(Problem));
            return problem;
        }

		public async Task<int> GetLessonId(int problemId)
		{
            Problem problem = await problemRepository.FindAsync(problemId);
			return problem.LessonId;
		}

        public string GetProblemName(int id)
        {
            string problemName = problemRepository.All()
                .Where(x => x.Id == id)
                .Select(x => x.Name)
                .FirstOrDefault();
            return problemName;
        }

        public int GetTimeIntevalBetweenSubmissionInSeconds(int problemId) => 
            problemRepository.All()
                .Where(p => p.Id == problemId)
                .Select(p => p.TimeIntervalBetweenSubmissionInSeconds)
                .First();

        public IEnumerable<LessonProblemViewModel> LessonProblems(int lessonId)
		{
			var problems = problemRepository
                .All()
				.Where(p => p.LessonId == lessonId)
                .OrderBy(p => p.OrderBy)
				.To<LessonProblemViewModel>()
				.ToList();
            return problems;
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
                .OrderBy(p => p.OrderBy)
				.To<SearchProblemViewModel>()
				.ToList();

            return results;
		}

		public async Task<ProblemDto> Update(ProblemEditInputModel model)
		{
            Problem problem = await problemRepository.FindAsync(model.Id);

			problem.Name = model.Name;
			problem.MaxPoints = model.MaxPoints;
			problem.IsExtraTask = model.IsExtraTask;
			problem.SubmissionType = model.SubmissionType;
            problem.AllowedTimeInMilliseconds = model.AllowedTimeInMilliseconds;
            problem.AllowedMemoryInMegaBytes = model.AllowedMemoryInMegaBytes;
            problem.TimeIntervalBetweenSubmissionInSeconds = model.TimeIntervalBetweenSubmissionInSeconds;
            problem.TestingStrategy = model.TestingStrategy;
            problem.AllowedMinCodeDifferenceInPercentage = model.AllowedMinCodeDifferenceInPercentage;
            problem.OrderBy = model.OrderBy;

            await problemRepository.UpdateAsync(problem);
            return problem.To<ProblemDto>();
		}

        public async Task AddAutomatedTestingProject(int id, byte[] testingProject)
        {
            Problem problem = await problemRepository.FindAsync(id);
            problem.AutomatedTestingProject = testingProject;
            await problemRepository.UpdateAsync(problem);
        }

        public async Task<byte[]> GetAutomatedTestingProject(int id) =>
            await problemRepository.All()
            .Where(p => p.Id == id)
            .Select(p => p.AutomatedTestingProject)
            .FirstOrDefaultAsync();
    }
}
