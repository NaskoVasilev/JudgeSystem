using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

using JudgeSystem.Web.Dtos.Common;

namespace JudgeSystem.Services
{
    public interface IFileSystemService
    {
        string CreateDirectory(string directoryPath, string name);

        void DeleteDirectory(string directoryPath);

        void AddFilesInDirectory(string directoryPath, IEnumerable<FileDto> files);

        Task CreateFile(Stream stream, string filePath);

        Task CreateFile(byte[] fileData, string filePath);

        void ExtractZipToDirectory(string filePath, string destinationDirectory);

        void ExtractZipToDirectory(string filePath, string destinationDirectory, bool overwrite);
    }
}
