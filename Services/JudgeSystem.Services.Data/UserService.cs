namespace JudgeSystem.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using JudgeSystem.Data.Common.Repositories;
    using JudgeSystem.Data.Models;
    using JudgeSystem.Data.Models.Enums;
    using JudgeSystem.Web.ViewModels.User;

    public class UserService : IUserService
	{
		private readonly IDeletableEntityRepository<ApplicationUser> repository;

		public UserService(
            IDeletableEntityRepository<ApplicationUser> repository)
		{
			this.repository = repository;
		}

		public List<UserCompeteResultViewModel> GetContestResults(string userId)
		{
			var result = repository.All()
				.Where(u => u.Id == userId)
				.SelectMany(u => u.UserContests)
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

		public List<UserPracticeResultViewModel> GetPracticetResults(string userId)
		{
            var result = repository.All()
                 .Where(u => u.Id == userId)
                 .SelectMany(u => u.UserPractices)
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
				.Select(uc => uc.Contest)
				.Where(c => c.Lesson.Type == LessonType.Exam)
				.Select(c => new UserCompeteResultViewModel
				{
					ContestName = c.Name,
					LessonId = c.LessonId,
					MaxPoints = c.Lesson.Problems.Where(p => !p.IsExtraTask).Sum(p => p.MaxPoints),
					ActualPoints = c.Submissions.Where(x => x.UserId == userId && !x.Problem.IsExtraTask).GroupBy(s => s.ProblemId)
					.Sum(submissionGroup => submissionGroup.Max(submission => submission.ActualPoints))
				})
				.ToList();

			return exams;
		}
    }
}
