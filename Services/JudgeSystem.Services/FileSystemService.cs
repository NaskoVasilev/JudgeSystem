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

        public async Task CreateFile(byte[] fileData, string filePath)
        {
            using var memoryStream = new MemoryStream(fileData);
            await CreateFile(memoryStream, filePath);
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

        public void ExtractZipToDirectory(string filePath, string destinationDirectory) =>
            ZipFile.ExtractToDirectory(filePath, destinationDirectory);

        public void ExtractZipToDirectory(string filePath, string destinationDirectory, bool overwrite)
        {
            if (!overwrite)
            {
                ExtractZipToDirectory(filePath, destinationDirectory);
                return;
            }

            using FileStream stream = File.Open(filePath, FileMode.Open);
            using var zip = new ZipArchive(stream, ZipArchiveMode.Read);
            foreach (ZipArchiveEntry file in zip.Entries)
            {
                string completeFileName = Path.GetFullPath(Path.Combine(destinationDirectory, file.FullName));

                // Assuming Empty for Directory
                if (file.Name == "")
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                    continue;
                }

                file.ExtractToFile(completeFileName, true);
            }
        }
    }
}
