using System.IO;
using System.Linq;

using JudgeSystem.Common;

namespace JudgeSystem.Services
{
    public class ValidationService : IValidationService
    {
        public bool IsValidFileExtension(string fileName)
        {
            string[] allowedExtensions = GlobalConstants.AllowedFileExtensoins;
            string fileExtension = Path.GetExtension(fileName);
            return allowedExtensions.Contains(fileExtension);
        }

        public bool IsValidFileExtension(string fileName, params string[] extensions)
        {
            string fileExtension = Path.GetExtension(fileName);
            return extensions.Contains(fileExtension);
        }
    }
}
