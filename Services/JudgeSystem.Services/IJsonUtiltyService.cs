using System.Collections.Generic;
using System.IO;

namespace JudgeSystem.Services
{
    public interface IJsonUtiltyService
    {
        public T PasreJsonFormStreamUsingJSchema<T>(Stream stream, string schemaFilePath, List<string> messages);
    }
}
