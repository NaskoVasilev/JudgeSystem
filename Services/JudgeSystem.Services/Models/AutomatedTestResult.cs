using JudgeSystem.Web.Dtos.Test;

namespace JudgeSystem.Services.Models
{
    public class AutomatedTestResult
    {
        public AutomatedTestResult(string ouput, bool isCorrect, TestDataDto testData)
        {
            Ouput = ouput;
            IsCorrect = isCorrect;
            TestData = testData;
        }

        public string Ouput { get; set; }

        public bool IsCorrect { get; set; }

        public TestDataDto TestData { get; set; }
    }
}
