using System.IO;
using System.Threading.Tasks;

namespace JudgeSystem.Services
{
    public interface IAzureStorageService
    {
        Task<string> Upload(Stream stream, string inputFileName, string containerName);

        Task Download(string filePath, Stream stream);

        Task Delete(string filePath);
    }
}
