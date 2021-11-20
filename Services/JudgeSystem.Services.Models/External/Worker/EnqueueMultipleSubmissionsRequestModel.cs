using System.Collections.Generic;

namespace JudgeSystem.Services.Models.External.Worker
{
    public class EnqueueMultipleSubmissionsRequestModel
    {
        public IEnumerable<EnqueueSubmissionRequestModel> Submissions { get; set; }
    }
}
