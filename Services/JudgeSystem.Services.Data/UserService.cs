using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Data.Common.Repositories;
using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Web.ViewModels.User;

namespace JudgeSystem.Services.Data
{
    public class UserService : IUserService
	{
		private readonly IDeletableEntityRepository<ApplicationUser> repository;
        private readonly IRepository<UserPractice> userPracticeReository;
        private readonly IRepository<UserContest> userContestReository;
        private readonly IStudentService studentService;

        public UserService(
            IDeletableEntityRepository<ApplicationUser> repository,
            IRepository<UserPractice> userPracticeReository,
            IRepository<UserContest> userContestReository,
            IStudentService studentService)
		{
			this.repository = repository;
            this.userPracticeReository = userPracticeReository;
            this.userContestReository = userContestReository;
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
            var userContests = userContestReository.All().Where(x => x.UserId == userId).ToList();

            foreach (var userContest in userContests)
            {
                userContestReository.DeleteAsync(userContest);
            }

            await userContestReository.SaveChangesAsync();
        }

        private async Task DeleteUserPractices(string userId)
        {
            var userPractices = userPracticeReository.All().Where(x => x.UserId == userId).ToList();

            foreach (var userPractice in userPractices)
            {
                userPracticeReository.DeleteAsync(userPractice);
            }

            await userPracticeReository.SaveChangesAsync();
        }
    }
}
