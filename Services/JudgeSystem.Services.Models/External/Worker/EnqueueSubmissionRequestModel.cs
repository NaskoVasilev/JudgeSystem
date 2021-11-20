using Newtonsoft.Json;

namespace JudgeSystem.Services.Models.External.Worker
{
    public class EnqueueSubmissionRequestModel
    {
        [JsonProperty("source_code")]
        public string SourceCode { get; set; }

        [JsonProperty("language_id")]
        public int LanguageId { get; set; }

        [JsonProperty("stdin")]
        public string StdIn { get; set; }
    }
}
