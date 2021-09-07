using System.Collections.Generic;
using System.IO;

namespace JudgeSystem.Services
{
    public interface IJsonUtiltyService
    {
        T ParseJsonFormStreamUsingJSchema<T>(Stream stream, string schemaFilePath, ICollection<string> messages);
    }
}
