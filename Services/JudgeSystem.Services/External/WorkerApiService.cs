using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Common.Helpers;
using JudgeSystem.Common.Settings;
using JudgeSystem.Services.Models.External.Worker;

namespace JudgeSystem.Services.External
{
    public class WorkerApiService : IWorkerApiService
    {
        private const string EnqueueSubmissionsRoute = "submissions/batch?base64_encoded=false";
        private const string GetSubmissionsResultPath = "submissions/batch?tokens={0}&base64_encoded=true";
        private readonly IHttpClientService httpClient;
        private readonly WorkerApiSettings workerApiSettings;

        public WorkerApiService(
            IHttpClientService httpClient,
            WorkerApiSettings workerApiSettings)
        {
            this.httpClient = httpClient;
            this.workerApiSettings = workerApiSettings;
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
                PrepenedBaseUrl(EnqueueSubmissionsRoute));

        public async Task<MultipleSubmissionsResultsResponseModel> GetSubmissionResult(IEnumerable<string> tokens)
            => await httpClient.Get<MultipleSubmissionsResultsResponseModel>(
                PrepenedBaseUrl(string.Format(GetSubmissionsResultPath, string.Join(",", tokens))));

        private string PrepenedBaseUrl(string route) => UrlHelper.Combine(workerApiSettings.BaseUrl, route);
    }
}
