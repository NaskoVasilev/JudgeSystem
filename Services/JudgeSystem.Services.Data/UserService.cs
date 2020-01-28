using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services.Mapping;
using JudgeSystem.Web.ViewModels.User;

using Microsoft.EntityFrameworkCore;

namespace JudgeSystem.Services.Data
{
    public class UserService : IUserService
	{
		private readonly IDeletableEntityRepository<ApplicationUser> repository;
        private readonly IRepository<UserPractice> userPracticeRepository;
        private readonly IRepository<UserContest> userContestRepository;
        private readonly IStudentService studentService;

        public UserService(
            IDeletableEntityRepository<ApplicationUser> repository,
            IRepository<UserPractice> userPracticeRepository,
            IRepository<UserContest> userContestRepository,
            IStudentService studentService)
		{
			this.repository = repository;
            this.userPracticeRepository = userPracticeRepository;
            this.userContestRepository = userContestRepository;
            this.studentService = studentService;
        }

		public List<UserCompeteResultViewModel> GetContestResults(string userId)
		{
			var result = repository.All()
				.Where(u => u.Id == userId)
				.SelectMany(u => u.UserContests)
                .Where(uc => uc.Contest.Lesson.Problems.Count > 0)
				.Select(uc => new UserCompeteResultViewModel()
				{
					ActualPoints = uc.Contest.Submissions
					.Where(s => s.UserId == userId && !s.Problem.IsExtraTask)
					.GroupBy(s => s.ProblemId)
					.Sum(x => x.Max(s => s.ActualPoints)),
					MaxPoints = uc.Contest.Lesson.Problems
					.Where(p => !p.IsExtraTask)
					.Sum(p => p.MaxPoints),
					ContestName = uc.Contest.Name,
					LessonId = uc.Contest.LessonId,
                    ContestId = uc.Contest.Id
				})
				.ToList();

			return result;
		}

		public List<UserPracticeResultViewModel> GetPracticeResults(string userId)
		{
            var result = repository.All()
                 .Where(u => u.Id == userId)
                 .SelectMany(u => u.UserPractices)
                 .Where(uc => uc.Practice.Lesson.Problems.Count > 0)
                 .Select(up => new UserPracticeResultViewModel()
                 {
                     ActualPoints = up.Practice.Submissions
                     .Where(s => s.UserId == userId && !s.Problem.IsExtraTask)
                     .GroupBy(s => s.ProblemId)
                     .Sum(x => x.Max(s => s.ActualPoints)),
                     MaxPoints = up.Practice.Lesson.Problems
                     .Where(p => !p.IsExtraTask)
                     .Sum(p => p.MaxPoints),
                     LessonName = up.Practice.Lesson.Name,
                     PracticeId = up.Practice.Id,
                     LessonId = up.Practice.LessonId
                 })
                 .ToList();

            return result;
		}

		public IEnumerable<UserCompeteResultViewModel> GetUserExamResults(string userId)
		{
			var exams = repository.All()
				.Where(u => u.Id == userId)
				.SelectMany(u => u.UserContests)
                .Where(uc => uc.Contest.Lesson.Problems.Count > 0)
                .Select(uc => uc.Contest)
				.Where(c => c.Lesson.Type == LessonType.Exam)
				.Select(c => new UserCompeteResultViewModel
				{
                    ContestId = c.Id,
					ContestName = c.Name,
					LessonId = c.LessonId,
					MaxPoints = c.Lesson.Problems.Where(p => !p.IsExtraTask).Sum(p => p.MaxPoints),
					ActualPoints = c.Submissions.Where(x => x.UserId == userId && !x.Problem.IsExtraTask).GroupBy(s => s.ProblemId)
					.Sum(submissionGroup => submissionGroup.Max(submission => submission.ActualPoints))
				})
				.ToList();

			return exams;
		}

        public async Task DeleteUserData(string userId, string studentId)
        {
            await DeleteUserPractices(userId);
            await DeleteUserContests(userId);
            if(studentId != null)
            {
                await studentService.Delete(studentId);
            }
        }

        private async Task DeleteUserContests(string userId)
        {
            var userContests = userContestRepository.All().Where(x => x.UserId == userId).ToList();
            await userContestRepository.DeleteRangeAsync(userContests);
        }

        private async Task DeleteUserPractices(string userId)
        {
            var userPractices = userPracticeRepository.All().Where(x => x.UserId == userId).ToList();
            await userPracticeRepository.DeleteRangeAsync(userPractices);
        }

        public async Task<UserNamesViewModel> GetUserNames(string userId)
        {
            UserNamesViewModel userNamesDto =await repository.All()
                .Where(x => x.Id == userId)
                .To<UserNamesViewModel>()
                .FirstOrDefaultAsync();
            return userNamesDto;
        }

        public IEnumerable<UserViewModel> All() => 
            repository.All()
            .To<UserViewModel>()
            .OrderBy(x => x.Name)
            .ThenBy(x => x.Surname)
            .ToList();

        public bool IsExistingUserWithNotConfirmedEmail(string username) => 
            repository.All().Any(x => x.UserName == username && !x.EmailConfirmed);
    }
}
