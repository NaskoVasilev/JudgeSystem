using System.Collections.Generic;

using JudgeSystem.Services.Models;
using JudgeSystem.Web.Dtos.Test;
using JudgeSystem.Workers.Common;

namespace JudgeSystem.Services
{
    public interface IChecker
    {
        CheckerResult Check(ExecutionResult executionResult, string expectedOutput);

        IEnumerable<AutomatedTestResult> CheckAutomatedTestsOutput(IEnumerable<TestDataDto> tests, ProcessResult testResults, string workingDirectory);
    }
}
