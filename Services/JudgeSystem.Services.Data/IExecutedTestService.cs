using System.Collections.Generic;
using System.Threading.Tasks;
using JudgeSystem.Data.Models;

namespace JudgeSystem.Services.Data
{
	public interface IExecutedTestService
	{
		Task Create(ExecutedTest executedTest);

		Task DeleteExecutedTestsByTestId(int testId);
	}
}
