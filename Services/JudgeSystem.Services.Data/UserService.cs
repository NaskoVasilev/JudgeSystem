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
		private readonly ILessonService lessonService;

		public UserService(
            IDeletableEntityRepository<ApplicationUser> repository, 
            ILessonService lessonService)
		{
			this.repository = repository;
			this.lessonService = lessonService;
		}

		public List<UserCompeteResultViewModel> GetContestResults(string userId)
		{
			var result = repository.All()
				.Where(u => u.Id == userId)
				.SelectMany(u => u.UserContests)
				.Select(uc => new UserCompeteResultViewModel()
				{
					ActualPoints = uc.Contest.Submissions
					.Where(s => !s.Problem.IsExtraTask)
					.GroupBy(s => s.ProblemId)
					.Sum(x => x.Max(s => s.ActualPoints)),
					MaxPoints = uc.Contest.Lesson.Problems
					.Where(p => !p.IsExtraTask)
					.Sum(p => p.MaxPoints),
					ContestName = uc.Contest.Name,
					LessonId = uc.Contest.LessonId
				})
				.ToList();

			return result;
		}

		public List<UserPracticeResultViewModel> GetPracticetResults(string userId)
		{
            //TODO: optimize this query
            //var result = repository.All()
            //	.Where(u => u.Id == userId)
            //	.SelectMany(u => u.Submissions)
            //	.GroupBy(s => s.Problem.Lesson)
            //	.Select(group => new UserPracticeResultViewModel()
            //	{
            //		LessonId = group.Key.Id,
            //		LessonName = group.Key.Name,
            //		ActualPoints = group.Where(s => !s.Problem.IsExtraTask)
            //		.GroupBy(s => s.Problem.Id)
            //		.Sum(submissionGroup => submissionGroup.Max(submission => submission.ActualPoints)),
            //		MaxPoints = group.Max(s => s.Problem.Lesson.Problems.Where(p => !p.IsExtraTask).Sum(p => p.MaxPoints))
            //	})
            //	.ToList();
            var result = repository.All()
                 .Where(u => u.Id == userId)
                 .SelectMany(u => u.UserPractices)
                 .Select(up => new UserPracticeResultViewModel()
                 {
                     ActualPoints = up.Practice.Submissions
                     .Where(s => !s.Problem.IsExtraTask)
                     .GroupBy(s => s.ProblemId)
                     .Sum(x => x.Max(s => s.ActualPoints)),
                     MaxPoints = up.Practice.Lesson.Problems
                     .Where(p => !p.IsExtraTask)
                     .Sum(p => p.MaxPoints),
                     LessonName = up.Practice.Lesson.Name,
                     LessonId = up.Practice.LessonId,
                     PracticeId = up.Practice.Id
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
					ActualPoints = c.Submissions.GroupBy(s => s.ProblemId)
					.Sum(submissionGroup => submissionGroup.Max(submission => submission.ActualPoints))
				})
				.ToList();

			return exams;
		}
	}
}
