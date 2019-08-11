using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Common.Exceptions;
using JudgeSystem.Web.Dtos.Submission;

using Microsoft.AspNetCore.Http;

namespace JudgeSystem.Services
{
    public class UtilityService : IUtilityService
    {
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

        public async Task<SubmissionCodeDto> ExtractSubmissionCode(string code, IFormFile submissionFile)
        {
            var submissionCodeDto = new SubmissionCodeDto();

            if (!string.IsNullOrEmpty(code))
            {
                if (code.Length > GlobalConstants.MaxSubmissionCodeLength)
                {
                    throw new BadRequestException(ErrorMessages.TooLongSubmissionCode);
                }
                submissionCodeDto.SourceCodes = new List<string> { code };
                submissionCodeDto.Content = Encoding.UTF8.GetBytes(code);
            }
            else if (submissionFile != null)
            {
                submissionCodeDto = await ExtractSubmissionCodeDtoFromSubmissionFile(submissionFile);
            }
            else
            {
                throw new BadRequestException();
            }

            return submissionCodeDto;
        }

        public List<string> ExtractZipFile(Stream stream, List<string> allowedFilesExtensions)
        {
            using (var zip = new ZipArchive(stream, ZipArchiveMode.Read))
            {
                return ExtractFilesFromZipArchive(allowedFilesExtensions, zip);
            }
        }

        private List<string> ExtractFilesFromZipArchive(List<string> allowedFilesExtensions, ZipArchive zip)
        {
            var filesData = new List<string>();

            foreach (ZipArchiveEntry entry in zip.Entries)
            {
                if (allowedFilesExtensions.Any(extension => entry.Name.EndsWith(extension)))
                {
                    string data = ExtractDataFromZipArchiveEntry(entry);
                    filesData.Add(data);
                }
            }

            return filesData;
        }

        private string ExtractDataFromZipArchiveEntry(ZipArchiveEntry entry)
        {
            using (var reader = new StreamReader(entry.Open()))
            {
                string data = reader.ReadToEnd();
                return data;
            }
        }

        private async Task<SubmissionCodeDto> ExtractSubmissionCodeDtoFromSubmissionFile(IFormFile submissionFile)
        {
            if (ConvertBytesToKiloBytes(submissionFile.Length) > GlobalConstants.SubmissionFileMaxSizeInKb)
            {
                throw new BadRequestException(ErrorMessages.TooBigSubmissionFile);
            }

            using (var stream = new MemoryStream())
            {
                await submissionFile.CopyToAsync(stream);
                return new SubmissionCodeDto
                {
                    Content = stream.ToArray(),
                    SourceCodes = ExtractZipFile(stream, new List<string> { GlobalConstants.cSharpFileExtension })
                };
            }
        }
    }
}
