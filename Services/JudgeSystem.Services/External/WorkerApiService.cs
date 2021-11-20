using JudgeSystem.Services.Models.External.Worker;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JudgeSystem.Services.External
{
    public class WorkerApiService : IWorkerApiService
    {
        private const string EnqueueSubmissionsPath = "submissions/batch?base64_encoded=false";
        private const string GetSubmissionsResultPath = "submissions/batch?tokens={0}&base64_encoded=true";
        private readonly IHttpClientService httpClient;

        public WorkerApiService(IHttpClientService httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<EnqueueSubmissionResponseModel>> Enqueue(string code, IEnumerable<string> tests, int languageId)
            => await httpClient.Post<IEnumerable<EnqueueSubmissionResponseModel>>(
                new EnqueueMultipleSubmissionsRequestModel
                {
                    Submissions = tests.Select(tests => new EnqueueSubmissionRequestModel
                    {
                        LanguageId = languageId,
                        SourceCode = code,
                        StdIn = tests,
                    })
                },
                EnqueueSubmissionsPath);


        public async Task<MultipleSubmissionsResultsResponseModel> GetSubmissionResult(IEnumerable<string> tokens)
            => await httpClient.Get<MultipleSubmissionsResultsResponseModel>(
                string.Format(GetSubmissionsResultPath, string.Join(",", tokens)));
    }
}
