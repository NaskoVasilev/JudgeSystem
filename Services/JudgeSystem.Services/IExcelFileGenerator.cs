using System;
using System.Collections.Generic;

using JudgeSystem.Web.ViewModels.Contest;

namespace JudgeSystem.Services
{
    public interface IExcelFileGenerator
    {
        byte[] Generate(List<string> columns, object[,] data);

        byte[] GenerateContestResultsReport(ContestAllResultsViewModel contestResultsData, List<string> columns);
    }
}
