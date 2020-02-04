namespace JudgeSystem.Services
{
    public interface IExcelFileGenerator
    {
        byte[] Generate(string[] columns, string[,] data);
    }
}
