using JudgeSystem.Services.Models.External.Worker;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JudgeSystem.Services.External
{
    public interface IWorkerApiService
    {
        Task<IEnumerable<EnqueueSubmissionResponseModel>> Enqueue(string code, IEnumerable<string> tests, int languageId);

        Task<MultipleSubmissionsResultsResponseModel> GetSubmissionResult(IEnumerable<string> tokens);
    }
}
