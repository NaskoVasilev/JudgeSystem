using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using JudgeSystem.Web.Dtos.Submission;

using Microsoft.AspNetCore.Http;

namespace JudgeSystem.Services
{
    public interface IUtilityService
    {
        Task<SubmissionCodeDto> ExtractSubmissionCode(string code, IFormFile submissionFile);

        List<string> ExtractZipFile(Stream stream, List<string> allowedFilesExtensions);

        double ConvertBytesToMegaBytes(long bytes);

        double ConvertBytesToKiloBytes(long bytes);

        int ConvertMegaBytesToBytes(double megabytes);
    }
}
