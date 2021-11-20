using System.Collections.Generic;

namespace JudgeSystem.Services.Models.External.Worker
{
    public class MultipleSubmissionsResultsResponseModel
    {
        public IEnumerable<SubmissionResultResponseModel> Submissions { get; set; }
    }
}
