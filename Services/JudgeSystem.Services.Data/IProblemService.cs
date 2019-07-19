namespace JudgeSystem.Services.Data
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	using JudgeSystem.Data.Models;
	using JudgeSystem.Web.InputModels.Problem;
	using JudgeSystem.Web.ViewModels.Problem;
	using JudgeSystem.Web.ViewModels.Search;

	public  interface IProblemService
	{
		Task<Problem> Create(ProblemInputModel model);

		IEnumerable<LessonProblemViewModel> LessonProblems(int lessonId);

		Task<Problem> GetById(int id);

		Task<Problem> GetByIdWithTests(int id);

		Task<Problem> Update(ProblemEditInputModel model);

		Task Delete(Problem problem);

		IEnumerable<SearchProblemViewModel> SearchByName(string keyword);

		int GetProblemMaxPoints(int id);

		Task<int> GetLessonId(int problemId);
	}
}
