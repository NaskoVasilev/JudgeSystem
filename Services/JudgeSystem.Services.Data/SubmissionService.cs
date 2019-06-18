namespace JudgeSystem.Services.Data
{
	using JudgeSystem.Common;
	using JudgeSystem.Data.Common.Repositories;
	using JudgeSystem.Data.Models;
	using JudgeSystem.Services.Mapping;
	using JudgeSystem.Web.Dtos.ExecutedTest;
	using JudgeSystem.Web.Dtos.Submission;
	using JudgeSystem.Web.InputModels.Submission;
	using JudgeSystem.Web.ViewModels.Submission;
	using Microsoft.EntityFrameworkCore;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class SubmissionService : ISubmissionService
	{
		private readonly IRepository<Submission> repository;
		private readonly IEstimator estimator;

		public SubmissionService(IRepository<Submission> repository, IEstimator estimator)
		{
			this.repository = repository;
			this.estimator = estimator;
		}

		public async Task<Submission> Create(SubmissionInputModel model, string userId)
		{
			Submission submission = new Submission
			{
				Code = Encoding.UTF8.GetBytes(model.Code),
				ProblemId = model.ProblemId,
				UserId = userId,
				ContestId = model.ContestId
			};

			await repository.AddAsync(submission);
			await repository.SaveChangesAsync();
			return submission;
		}

		public SubmissionResult GetSubmissionResult(int id)
		{
			var submissioResult = repository.All()
				.Where(s => s.Id == id)
				.Include(s => s.ExecutedTests)
				.Include(s => s.Problem)
				.Select(MapSubmissionToSubmissionResult)
				.FirstOrDefault();

			int passedTests = submissioResult.ExecutedTests.Count(t => t.IsCorrect);
			submissioResult.ActualPoints = estimator.CalculteProblemPoints(submissioResult.ExecutedTests.Count, passedTests, submissioResult.MaxPoints);

			return submissioResult;
		}

		public IEnumerable<SubmissionResult> GetUserSubmissionsByProblemId(int problemId, string userId, int page, int submissionsPerPage)
		{
			var submissionsFromDb = repository.All()
				.Where(s => s.ProblemId == problemId && s.UserId == userId);

			var submissions = GetSubmissionResults(submissionsFromDb, page, submissionsPerPage);
			return submissions;
		}

		public async Task Update(Submission submission)
		{
			repository.Update(submission);
			await repository.SaveChangesAsync();
		}

		public SubmissionResult MapSubmissionToSubmissionResult(Submission submission)
		{
			SubmissionResult submissionResult = new SubmissionResult
			{
				MaxPoints = submission.Problem.MaxPoints,
				ExecutedTests = submission.ExecutedTests.Select(t => new ExecutedTestResult
				{
					IsCorrect = t.IsCorrect,
					ExecutionResultType = t.ExecutionResultType.ToString()
				})
					.ToList(),
				IsCompiledSuccessfully = submission.CompilationErrors == null || submission.CompilationErrors.Length == 0,
				SubmissionDate = submission.SubmisionDate.ToString(GlobalConstants.StandardDateFormat, CultureInfo.InvariantCulture),
				Id = submission.Id,
				TotalMemoryUsed = submission.ExecutedTests.Sum(t => t.MemoryUsed),
				TotalTimeUsed = submission.ExecutedTests.Sum(t => t.TimeUsed)
			};

			return submissionResult;
		}

		public int GetProblemSubmissionsCount(int problemId, string userId)
		{
			return repository.All().Count(s => s.ProblemId == problemId && s.UserId == userId);
		}

		public int GetSubmissionsCountByProblemIdAndContestId(int problemId, int contestId, string userId)
		{
			return repository.All().Count(s => s.ProblemId == problemId && s.UserId == userId && s.ContestId == contestId);
		}

		public SubmissionViewModel GetSubmissionDetails(int id)
		{
			var submission = this.repository.All()
				.Where(s => s.Id == id)
				.To<SubmissionViewModel>()
				.FirstOrDefault();

			submission.ExecutedTests = submission.ExecutedTests.OrderByDescending(t => t.TestIsTrialTest).ToList();
			return submission;
		}

		public IEnumerable<SubmissionResult> GetUserSubmissionsByProblemIdAndContestId(int contestId, int problemId, string userId, int page, int submissionsPerPage)
		{
			var submissionsFromDb = repository.All()
				.Where(s => s.ContestId == contestId && s.UserId == userId && problemId == problemId);

			var submissions = GetSubmissionResults(submissionsFromDb, page, submissionsPerPage);
			return submissions;
		}

		private IEnumerable<SubmissionResult> GetSubmissionResults(IQueryable<Submission> submissionsFromDb, int page, int submissionsPerPage)
		{
			var submissions = submissionsFromDb
				.Include(s => s.ExecutedTests)
				.Include(s => s.Problem)
				.OrderByDescending(s => s.SubmisionDate)
				.Skip((page - 1) * submissionsPerPage)
				.Take(submissionsPerPage)
				.Select(MapSubmissionToSubmissionResult)
				.ToList();

			foreach (var submission in submissions)
			{
				int passedTests = submission.ExecutedTests.Count(t => t.IsCorrect);
				submission.ActualPoints = estimator.CalculteProblemPoints(submission.ExecutedTests.Count, passedTests, submission.MaxPoints);
			}

			return submissions;
		}
	}
}
