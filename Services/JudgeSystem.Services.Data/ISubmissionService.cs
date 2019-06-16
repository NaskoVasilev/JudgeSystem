namespace JudgeSystem.Services.Data
{
	using System.Threading.Tasks;

	using JudgeSystem.Web.ViewModels.Submission;
	using JudgeSystem.Web.InputModels.Submission;
	using JudgeSystem.Data.Models;

	public interface ISubmissionService
	{
		Task<Submission> Create(SubmissionInputModel model, string userId);

		Task Update(Submission submission);
	}
}
