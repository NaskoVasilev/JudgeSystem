using System.Collections.Generic;
using System.Threading.Tasks;

using JudgeSystem.Web.Dtos.Test;
using JudgeSystem.Web.ViewModels.Test;
using JudgeSystem.Web.InputModels.Test;
using JudgeSystem.Web.InputModels.Problem;

namespace JudgeSystem.Services.Data
{
    public interface ITestService
	{
		Task<TestDto> Add(TestInputModel model);

        Task AddRange(IEnumerable<ProblemTestInputModel> tests, int problemId);

		IEnumerable<T> GetTestsByProblemIdOrderedByIsTrialDescending<T>(int problemId);

		Task<TDestination> GetById<TDestination>(int id);

		Task Delete(int id);

		Task Update(TestEditInputModel test);
	}
}
