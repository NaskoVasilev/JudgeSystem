using System.IO;
using System.Threading.Tasks;

namespace JudgeSystem.Services
{
    public interface IFileStorageService
    {
        Task<string> Upload(Stream stream, string inputFileName, string folderName);

        Task Download(string filePath, Stream stream);

        Task Delete(string filePath);
    }
}
