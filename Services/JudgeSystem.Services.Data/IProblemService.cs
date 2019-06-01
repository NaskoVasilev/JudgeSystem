namespace JudgeSystem.Services.Data
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	using JudgeSystem.Data.Models;
	using JudgeSystem.Web.ViewModels.Problem;

	public  interface IProblemService
	{
		Task<Problem> Create(ProblemInputModel model);

		IEnumerable<LessonProblemViewModel> LesosnProblems(int lessonId);

		Task<Problem> GetById(int id);

		Task<Problem> GetByIdWithTests(int id);

		Task<Problem> Update(ProblemEditInputModel model);

		Task Delete(Problem problem);
	}
}
