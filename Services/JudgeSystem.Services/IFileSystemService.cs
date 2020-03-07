using JudgeSystem.Common;
using JudgeSystem.Web.Dtos.Common;
using System.Collections.Generic;

namespace JudgeSystem.Services
{
    public interface IFileSystemService
    {
        string CreateDirectory(string directoryPath, string name);

        void DeleteDirectory(string directoryPath);

        void AddFilesInDirectory(string directoryPath, IEnumerable<FileDto> files);
    }
}
