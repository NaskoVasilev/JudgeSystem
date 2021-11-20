using System.IO;
using System.Threading.Tasks;

using JudgeSystem.Services;

namespace JudgeSystem.Web.Tests.Mocks
{
    public class AzureStorageServiceMock : IFileStorageService
    {
        public static string FilePath;

        public AzureStorageServiceMock()
        {
            FilePath = string.Empty;
        }

        public Task Delete(string filePath)
        {
            FilePath = filePath;
            return Task.CompletedTask;
        }

        public Task Download(string filePath, Stream stream)
        {
            FilePath = filePath;
            return Task.CompletedTask;
        }

        public Task<string> Upload(Stream stream, string inputFileName, string containerName) => Task.FromResult(ConstructFilePath(inputFileName, containerName));

        public string ConstructFilePath(string inputFileName, string containerName) => $"storage/{containerName}/{inputFileName}";
    }
}
