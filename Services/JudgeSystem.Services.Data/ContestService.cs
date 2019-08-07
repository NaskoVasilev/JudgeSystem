using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Common.Exceptions;
using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.Infrastructure.Pagination;
using JudgeSystem.Web.InputModels.Contest;
using JudgeSystem.Web.ViewModels.Contest;
using JudgeSystem.Web.ViewModels.Problem;
using JudgeSystem.Web.ViewModels.Student;

using Microsoft.EntityFrameworkCore;

namespace JudgeSystem.Services.Data
{
    public class ContestService : IContestService
	{
		public const int ContestsPerPage = 20;
		public const int ResultsPerPage = 15;

		private readonly IDeletableEntityRepository<Contest> repository;
		private readonly IRepository<UserContest> userContestRepository;
        private readonly ILessonService lessonService;
        private readonly IProblemService problemService;
        private readonly ISubmissionService submissionService;
        private readonly IPaginationService paginationService;

        public ContestService(
            IDeletableEntityRepository<Contest> repository,
			IRepository<UserContest> userContestRepository,
            ILessonService lessonService,
            IProblemService problemService,
            ISubmissionService submissionService,
            IPaginationService paginationService)
		{
			this.repository = repository;
			this.userContestRepository = userContestRepository;
            this.lessonService = lessonService;
            this.problemService = problemService;
            this.submissionService = submissionService;
            this.paginationService = paginationService;
        }

		public async Task<bool> AddUserToContestIfNotAdded(string userId, int contestId)
		{
			if(this.userContestRepository.All().SingleOrDefault(uc => uc.UserId == userId && uc.ContestId == contestId) != null)
			{
				return false;
			}

			await userContestRepository.AddAsync(new UserContest { UserId = userId, ContestId = contestId });
			await userContestRepository.SaveChangesAsync();
			return true;
		}

        public async Task Create(ContestCreateInputModel contestCreateInputModel)
		{
            var contest = contestCreateInputModel.To<Contest>();
			await repository.AddAsync(contest);
			await repository.SaveChangesAsync();
		}

		public IEnumerable<ActiveContestViewModel> GetActiveContests()
		{
			var contests = repository.All()
				.Where(c => c.IsActive)
				.To<ActiveContestViewModel>()
				.ToList();
			return contests;
		}

		public async Task<T> GetById<T>(int contestId)
		{
            var contest = await repository.FindAsync(contestId);
			return contest.To<T>();
		}

		public IEnumerable<ContestBreifInfoViewModel> GetActiveAndFollowingContests()
		{
			var followingContests = this.repository.All()
				.Where(c => c.EndTime > DateTime.Now)
				.To<ContestBreifInfoViewModel>()
				.ToList();

			return followingContests;
		}

		public IEnumerable<PreviousContestViewModel> GetPreviousContests(int passedDays)
		{
			var contests = repository.All()
				.Where(c => c.EndTime < DateTime.Now && (DateTime.Now - c.EndTime).Days <= passedDays)
				.To<PreviousContestViewModel>()
                .ToList();
			return contests;
		}

		public async Task Update(ContestEditInputModel model)
		{
            Contest contest = await repository.FindAsync(model.Id);
            contest.Name = model.Name;
			contest.StartTime = model.StartTime;
			contest.EndTime = model.EndTime;

			repository.Update(contest);
			await repository.SaveChangesAsync();
		}

		public async Task Delete(int id)
		{
            Contest contest = await repository.FindAsync(id);
            repository.Delete(contest);
			await repository.SaveChangesAsync();
		}

		public IEnumerable<ContestViewModel> GetAllConests(int page)
		{
			var contests = repository.All()
				.OrderByDescending(c => c.StartTime)
				.Skip((page - 1) * ContestsPerPage)
				.Take(ContestsPerPage)
				.To<ContestViewModel>()
				.ToList();

			return contests;
		}

		public int GetNumberOfPages()
		{
			int numberOfContests = repository.All().Count();
			return (int)Math.Ceiling((double)numberOfContests / ContestsPerPage);
		}

		public ContestAllResultsViewModel GetContestReults(int contestId, int page)
		{
            var contestResults = repository.All()
				.Where(c => c.Id == contestId)
				.Select(c => new ContestAllResultsViewModel()
				{
					Id = c.Id,
					Name = c.Name,
					Problems = c.Lesson.Problems
					.OrderBy(p => p.CreatedOn)
					.Select(p => new ContestProblemViewModel
					{
						Id = p.Id,
						Name = p.Name
					})
					.ToList(),
					ContestResults = c.UserContests
					.Where(u => u.User.StudentId != null)
					.Select(uc => new ContestResultViewModel
					{
						Student = new StudentBreifInfoViewModel
						{
							ClassNumber = uc.User.Student.SchoolClass.ClassNumber,
							ClassType = uc.User.Student.SchoolClass.ClassType.ToString(),
							FullName = uc.User.Student.FullName,
							NumberInCalss = uc.User.Student.NumberInCalss
						},
						PointsByProblem = uc.User.Submissions
						.Where(s => s.ContestId == contestId)
						.GroupBy(s => s.ProblemId)
						.ToDictionary(s => s.Key, x => x.Max(s => s.ActualPoints))
					})
					.OrderBy(cr => cr.Student.ClassNumber)
					.ThenBy(cr => cr.Student.ClassType)
					.ThenBy(cr => cr.Student.NumberInCalss)
					.Skip((page - 1) * ResultsPerPage)
					.Take(ResultsPerPage)
					.ToList(),
				})
				.FirstOrDefault();

			return contestResults;
		}

		public int GetContestResultsPagesCount(int contestId)
		{
            if(!this.repository.All().Any(x => x.Id == contestId))
            {
                throw new EntityNotFoundException("contest");
            }

			int count = repository.All()
				.Include(c => c.UserContests)
				.ThenInclude(uc => uc.User)
				.FirstOrDefault(c => c.Id == contestId)
				.UserContests
				.Where(uc => uc.User.StudentId != null)
				.Count();
			return (int)Math.Ceiling(count / (double)ResultsPerPage);
		}

        public async Task<int> GetLessonId(int contestId)
        {
            var contest = await this.repository.FindAsync(contestId);
            return contest.LessonId;
        }

        public async Task<ContestSubmissionsViewModel> GetContestSubmissions(int contestId, string userId, int? problemId, int page, string baseUrl)
        {
            int baseProblemId;
            int lessonId = await this.GetLessonId(contestId);
            if (problemId.HasValue)
            {
                baseProblemId = problemId.Value;
            }
            else
            {
                baseProblemId = lessonService.GetFirstProblemId(lessonId);
            }

            var submissions = submissionService.GetUserSubmissionsByProblemIdAndContestId(contestId, baseProblemId, userId, page, GlobalConstants.SubmissionsPerPage);
            string problemName = problemService.GetProblemName(baseProblemId);

            int submissionsCount = submissionService.GetSubmissionsCountByProblemIdAndContestId(baseProblemId, contestId, userId);

            PaginationData paginationData = new PaginationData
            {
                CurrentPage = page,
                NumberOfPages = paginationService.CalculatePagesCount(submissionsCount, GlobalConstants.SubmissionsPerPage),
                Url = baseUrl + $"{GlobalConstants.QueryStringDelimiter}{GlobalConstants.ProblemIdKey}={baseProblemId}{GlobalConstants.QueryStringDelimiter}{GlobalConstants.PageKey}=" + "{0}"
            };

            var model = new ContestSubmissionsViewModel
            {
                ProblemName = problemName,
                Submissions = submissions,
                LessonId = lessonId,
                UrlPlaceholder = baseUrl + $"{GlobalConstants.QueryStringDelimiter}{GlobalConstants.ProblemIdKey}=" + "{0}",
                PaginationData = paginationData
            };

            return model;
        }
    }
}
