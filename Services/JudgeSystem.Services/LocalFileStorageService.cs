using System.IO;
using System.Threading.Tasks;

namespace JudgeSystem.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        public Task Delete(string filePath) => throw new System.NotImplementedException();
        
        public Task Download(string filePath, Stream stream) => throw new System.NotImplementedException();
        
        public async Task<string> Upload(Stream stream, string inputFileName, string folderName)
        {
            string filePath = Path.Combine(folderName, Path.GetRandomFileName() + "." + Path.GetExtension(inputFileName));

            // TODO: upload

            return filePath;
        }
    }
}
