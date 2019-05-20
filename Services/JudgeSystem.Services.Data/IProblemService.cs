using JudgeSystem.Data.Models;
using JudgeSystem.Web.ViewModels.Problem;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JudgeSystem.Services.Data
{
	public  interface IProblemService
	{
		Task<Problem> Create(ProblemInputModel model);

		IEnumerable<LessonProblemViewModel> LesosnProblems(int lessonId);

		Task<Problem> GetById(int id);
	}
}
