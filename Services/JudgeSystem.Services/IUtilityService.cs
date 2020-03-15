using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using JudgeSystem.Web.Dtos.Common;
using JudgeSystem.Web.Dtos.Submission;
using JudgeSystem.Workers.Common;

using Microsoft.AspNetCore.Http;

namespace JudgeSystem.Services
{
    public interface IUtilityService
    {
        Task<SubmissionCodeDto> ExtractSubmissionCode(string code, IFormFile submissionFile, ProgrammingLanguage programmingLanguage);

        List<CodeFile> ExtractZipFile(Stream stream, ISet<string> allowedFileExtensions);

        IEnumerable<FileDto> ParseZip(Stream stream, ISet<string> allowedFilesExtensions = null);

        double ConvertBytesToMegaBytes(long bytes);

        double ConvertBytesToKiloBytes(long bytes);

        int ConvertMegaBytesToBytes(double megabytes);

        void CreateLanguageSpecificFiles(ProgrammingLanguage programmingLanguage, IEnumerable<CodeFile> sourceCodes, string workingDirectory);

        string GetJavaClassName(string sourceCode);

        string GetJavaMainClass(IEnumerable<string> sourceCodes);
    }
}
