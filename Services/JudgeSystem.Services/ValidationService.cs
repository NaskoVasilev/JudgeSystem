using System.IO;
using System.Linq;

namespace JudgeSystem.Services
{
    public class ValidationService : IValidationService
    {
        public bool IsValidFileExtension(string fileName)
        {
            string[] allowedExtensions = new string[] { ".ppt", ".pptx", ".doc", ".docx", ".xls", ".cs", ".zip", ".json", ".xml", ".mp4", ".avi", ".txt" };
            string fileExtension = Path.GetExtension(fileName);
            return allowedExtensions.Contains(fileExtension);
        }
    }
}
