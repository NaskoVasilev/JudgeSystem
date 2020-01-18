using System.Collections.Generic;
using System.IO;

namespace JudgeSystem.Services
{
    public interface IJsonUtiltyService
    {
        public T ParseJsonFormStreamUsingJSchema<T>(Stream stream, string schemaFilePath, List<string> messages);
    }
}
