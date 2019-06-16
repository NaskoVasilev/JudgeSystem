namespace JudgeSystem.Services.Data
{
	using System.Threading.Tasks;
	using JudgeSystem.Web.InputModels.Submission;
	using JudgeSystem.Data.Models;
	using JudgeSystem.Web.Dtos.Submission;
	using System.Collections.Generic;

	public interface ISubmissionService
	{
		Task<Submission> Create(SubmissionInputModel model, string userId);

		Task Update(Submission submission);

		IEnumerable<SubmissionResult> GetUserSubmissionsByProblemId(int problemId, string userId, int page, int submissionsPerPage);

		SubmissionResult GetSubmissionResult(int id);

		int GetProblemSubmissionsCount(int problemId, string userId);
	}
}
