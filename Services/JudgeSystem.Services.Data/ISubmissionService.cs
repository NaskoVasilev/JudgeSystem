using System.Collections.Generic;
using System.Threading.Tasks;

using JudgeSystem.Web.InputModels.Submission;
using JudgeSystem.Web.Dtos.Submission;
using JudgeSystem.Web.ViewModels.Submission;

namespace JudgeSystem.Services.Data
{
    public interface ISubmissionService
	{
		Task<SubmissionDto> Create(SubmissionInputModel model, string userId);

		IEnumerable<SubmissionResult> GetUserSubmissionsByProblemId(int problemId, string userId, int page, int submissionsPerPage);

		SubmissionResult GetSubmissionResult(int id);

		int GetProblemSubmissionsCount(int problemId, string userId);

		int GetSubmissionsCountByProblemIdAndContestId(int problemId, int contestId, string userId);

		SubmissionViewModel GetSubmissionDetails(int id);

		IEnumerable<SubmissionResult> GetUserSubmissionsByProblemIdAndContestId(int contestId, int problemId, string userId, int page, int submissionsPerPage);

		Task CalculateActualPoints(int submissionId);

        byte[] GetSubmissionCodeById(int id);

        string GetProblemNameBySubmissionId(int id);

        IEnumerable<SubmissionResult> GetUserSubmissionsByProblemIdAndPracticeId(int practiceId, int problemId, string userId, int page, int submissionsPerPage);

        int GetSubmissionsCountByProblemIdAndPracticeId(int problemId, int practiceId, string userId);

        Task ExecuteSubmission(int submissionId, List<string> sourceCodes);
    }
}
