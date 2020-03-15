using System.Collections.Generic;

namespace JudgeSystem.Services
{
    public interface ICodeCompareer
    {
        double GetMinCodeDifference(string sourceCode, IEnumerable<string> otherCodes);
    }
}
