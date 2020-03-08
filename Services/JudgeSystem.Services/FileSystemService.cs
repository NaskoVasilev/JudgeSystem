using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Collections.Generic;

using JudgeSystem.Web.Dtos.Common;

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

        public async Task CreateFile(Stream stream, string filePath)
        {
            using FileStream fileStream = File.Create(filePath);
            await stream.CopyToAsync(fileStream);
            await fileStream.FlushAsync();
        }

        public void DeleteDirectory(string workingDirectory)
        {
            IEnumerable<string> files = Directory.EnumerateFiles(workingDirectory);
            foreach (string file in files)
            {
                File.Delete(file);
            }

            IEnumerable<string> directories = Directory.EnumerateDirectories(workingDirectory);
            foreach (string directory in directories)
            {
                DeleteDirectory(directory);
            }

            Directory.Delete(workingDirectory);
        }

        public void ExtractZipToDirectory(string filePath, string projectDirectory) =>
            ZipFile.ExtractToDirectory(filePath, projectDirectory);
    }
}
