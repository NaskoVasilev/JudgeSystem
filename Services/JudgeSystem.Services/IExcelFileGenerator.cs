using System.Collections.Generic;

namespace JudgeSystem.Services
{
    public interface IExcelFileGenerator
    {
        byte[] Generate(List<string> columns, object[,] data);
    }
}
