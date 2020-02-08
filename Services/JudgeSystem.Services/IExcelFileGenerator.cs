using System.Collections.Generic;

using JudgeSystem.Web.ViewModels.Contest;
using JudgeSystem.Web.ViewModels.Practice;

namespace JudgeSystem.Services
{
    public interface IExcelFileGenerator
    {
        byte[] Generate(List<string> columns, object[,] data);

        byte[] GenerateContestResultsReport(ContestAllResultsViewModel contestResultsData, List<string> columns);

        byte[] GeneratePracticeResultsReport(PracticeAllResultsViewModel practiceResultsData, List<string> columns);
    }
}
