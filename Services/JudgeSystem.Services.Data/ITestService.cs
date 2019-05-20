using JudgeSystem.Data.Models;
using JudgeSystem.Web.ViewModels.Test;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JudgeSystem.Services.Data
{
	public interface ITestService
	{
		Task<Test> Add(TestInputModel model);

		IEnumerable<TestViewModel> TestsByProblem(int problemId);

		Task<Test> GetById(int id);
	}
}
