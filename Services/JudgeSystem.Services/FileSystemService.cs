using System.IO;
using System.Collections.Generic;

using JudgeSystem.Web.Dtos.Common;
using static JudgeSystem.Common.GlobalConstants;

namespace JudgeSystem.Services
{
    public class FileSystemService : IFileSystemService
    {
        public void AddFilesInDirectory(string directoryPath, IEnumerable<FileDto> files)
        {
            foreach (FileDto file in files)
            {
                string filePath = Path.Combine(directoryPath, file.Name);
                File.WriteAllText(filePath, file.Content);
            }
        }

        public string CreateDirectory(string path, string name)
        {
            string fullPath = Path.Combine(path, name);
            return Directory.CreateDirectory(fullPath).FullName;
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
    }
}
