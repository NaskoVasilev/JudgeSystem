using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Common.Exceptions;
using JudgeSystem.Web.Dtos.Submission;
using JudgeSystem.Workers.Common;

using Microsoft.AspNetCore.Http;

namespace JudgeSystem.Services
{
    public class UtilityService : IUtilityService
    {
        private const string InvalidJavaClassErrroMessage = "Java class in your solution is invalid.";

        public double ConvertBytesToMegaBytes(long bytes)
        {
            double megabyteInBytes = 1000000;
            return bytes / megabyteInBytes;
        }

        public double ConvertBytesToKiloBytes(long bytes)
        {
            double kilobyteInBytes = 1000;
            return bytes / kilobyteInBytes;
        }

        public int ConvertMegaBytesToBytes(double megabytes) => (int)(megabytes * 1000 * 1000);

        public async Task<SubmissionCodeDto> ExtractSubmissionCode(string code, IFormFile submissionFile, ProgrammingLanguage programmingLanguage)
        {
            var submissionCodeDto = new SubmissionCodeDto();

            if (!string.IsNullOrEmpty(code))
            {
                if (code.Length > GlobalConstants.MaxSubmissionCodeLength)
                {
                    throw new BadRequestException(ErrorMessages.TooLongSubmissionCode);
                }
                submissionCodeDto.SourceCodes = new List<CodeFile>
                {
                    new CodeFile { Name = Path.GetRandomFileName(), Code = code }
                };
                submissionCodeDto.Content = Encoding.UTF8.GetBytes(code);
            }
            else if (submissionFile != null)
            {
                submissionCodeDto = await ExtractSubmissionCodeDtoFromSubmissionFile(submissionFile, programmingLanguage);
            }
            else
            {
                throw new BadRequestException();
            }

            return submissionCodeDto;
        }

        public List<CodeFile> ExtractZipFile(Stream stream, List<string> allowedFilesExtensions)
        {
            using (var zip = new ZipArchive(stream, ZipArchiveMode.Read))
            {
                return ExtractFilesFromZipArchive(allowedFilesExtensions, zip);
            }
        }

        public void CreateLanguageSpecificFiles(ProgrammingLanguage programmingLanguage, IEnumerable<CodeFile> codeFiles, string workingDirectory)
        {
            string fileExtension = GetFileExtension(programmingLanguage);
            foreach (CodeFile codeFile in codeFiles)
            {
                string file = workingDirectory + codeFile.Name + fileExtension;
                File.WriteAllText(file, codeFile.Code);
            }
        }

        public void DeleteDirectory(string workingDirectory)
        {
            IEnumerable<string> files = Directory.EnumerateFiles(workingDirectory);
            foreach (string file in files)
            {
                File.Delete(file);
            }
            Directory.Delete(workingDirectory);
        }

        public string GetJavaMainClass(IEnumerable<string> sourceCodes)
        {
            string mainMethodRegexPattern = @"static void main\s*\([^)]*\)";
            var mainMethodRegex = new Regex(mainMethodRegexPattern);

            foreach (string sourceCode in sourceCodes)
            {
                if (mainMethodRegex.IsMatch(sourceCode))
                {
                    return sourceCode;
                }
            }
            return null;
        }

        public string GetJavaClassName(string sourceCode)
        {
            string pattern = @"class ([^\s\n{]+)";
            var regex = new Regex(pattern);
            if (!regex.IsMatch(sourceCode))
            {
                throw new BadRequestException(InvalidJavaClassErrroMessage);
            }

            Match match = regex.Match(sourceCode);
            return match.Groups[1].Value;
        }

        private List<CodeFile> ExtractFilesFromZipArchive(List<string> allowedFilesExtensions, ZipArchive zip)
        {
            var filesData = new List<CodeFile>();

            foreach (ZipArchiveEntry entry in zip.Entries)
            {
                if (allowedFilesExtensions.Any(extension => entry.Name.EndsWith(extension)))
                {
                    CodeFile codeFile = ExtractDataFromZipArchiveEntry(entry);
                    filesData.Add(codeFile);
                }
            }

            return filesData;
        }

        private CodeFile ExtractDataFromZipArchiveEntry(ZipArchiveEntry entry)
        {
            using (var reader = new StreamReader(entry.Open()))
            {
                var codeFile = new CodeFile() { Name = Path.GetFileNameWithoutExtension(entry.Name), Code = reader.ReadToEnd() };
                return codeFile;
            }
        }

        private async Task<SubmissionCodeDto> ExtractSubmissionCodeDtoFromSubmissionFile(IFormFile submissionFile, ProgrammingLanguage programmingLanguage)
        {
            if (ConvertBytesToKiloBytes(submissionFile.Length) > GlobalConstants.SubmissionFileMaxSizeInKb)
            {
                throw new BadRequestException(ErrorMessages.TooBigSubmissionFile);
            }
            if (programmingLanguage != ProgrammingLanguage.CSharp && programmingLanguage != ProgrammingLanguage.Java)
            {
                throw new BadRequestException(string.Format(ErrorMessages.UnsuportedZipSubmission, GetProgrammingLanguageDisplayName(programmingLanguage)));
            }

            using (var stream = new MemoryStream())
            {
                await submissionFile.CopyToAsync(stream);
                var fileExtensions = new List<string> { GetFileExtension(programmingLanguage) };

                List<CodeFile> sourceCodes = ExtractZipFile(stream, fileExtensions);
                if (sourceCodes.Count == 0)
                {
                    throw new BadRequestException(ErrorMessages.EmptyArchiveSubmitted);
                }

                return new SubmissionCodeDto
                {
                    Content = stream.ToArray(),
                    SourceCodes = sourceCodes
                };
            }
        }

        private string GetFileExtension(ProgrammingLanguage programmingLanguage)
        {
            if (programmingLanguage == ProgrammingLanguage.CSharp)
            {
                return GlobalConstants.CSharpFileExtension;
            }
            else if (programmingLanguage == ProgrammingLanguage.Java)
            {
                return GlobalConstants.JavaFileExtension;
            }
            else if (programmingLanguage == ProgrammingLanguage.CPlusPlus)
            {
                return GlobalConstants.CppFileExtension;
            }
            else if(programmingLanguage == ProgrammingLanguage.Python)
            {
                return GlobalConstants.PythonFileExtension;
            }

            return null;
        }

        private string GetProgrammingLanguageDisplayName(ProgrammingLanguage programmingLanguage)
        {
            switch (programmingLanguage)
            {
                case ProgrammingLanguage.CSharp:
                    return "C#";
                case ProgrammingLanguage.Java:
                    return "Java";
                case ProgrammingLanguage.CPlusPlus:
                    return "C++";
                default: return null;
            }
        }
    }
}
