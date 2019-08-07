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

        public int ConvertMegaBytesToBytes(double megabytes)
        {
            return (int)(megabytes * 1000 * 1000);
        }

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

        public  List<string> ExtractZipFile(Stream stream, List<string> allowedFilesExtensions)
        {
            List<string> filesData = new List<string>();

            using (ZipArchive zip = new ZipArchive(stream, ZipArchiveMode.Read))
            {
                foreach (ZipArchiveEntry entry in zip.Entries)
                {
                    if (allowedFilesExtensions.Any(extension => entry.Name.EndsWith(extension)))
                    {
                        using (StreamReader reader = new StreamReader(entry.Open()))
                        {
                            string data = reader.ReadToEnd();
                            filesData.Add(data);
                        }
                    }
                }
            }

            return filesData;
        }

        private async Task<SubmissionCodeDto> ExtractSubmissionCodeDtoFromSubmissionFile(IFormFile submissionFile)
        {
            if (this.ConvertBytesToKiloBytes(submissionFile.Length) > GlobalConstants.SubmissionFileMaxSizeInKb)
            {
                throw new BadRequestException(ErrorMessages.TooBigSubmissionFile);
            }

            using (var stream = new MemoryStream())
            {
                await submissionFile.CopyToAsync(stream);
                return new SubmissionCodeDto
                {
                    Content = stream.ToArray(),
                    SourceCodes = this.ExtractZipFile(stream, new List<string> { GlobalConstants.cSharpFileExtension })
                };
            }
        }
    }
}
