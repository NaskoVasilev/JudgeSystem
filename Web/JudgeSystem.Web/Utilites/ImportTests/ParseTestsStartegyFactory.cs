using System;

using JudgeSystem.Data.Models.Enums;

namespace JudgeSystem.Web.Utilites.ImportTests
{
    public class ParseTestsStartegyFactory<T>
    {
        private const string UnsupportedTestsImportStrategyErrorMessage = "This tests import strategy is not supported!";

        ParseTestsStrategy<T> CreateStrategy(TestsImportStrategy strategy)
        {
            switch (strategy)
            {
                case TestsImportStrategy.Json:
                    return new ParseTestsFromJson<T>();
                case TestsImportStrategy.Zip:
                    return new ParseTestsFromZip<T>();
                default:
                    throw new ArgumentException(UnsupportedTestsImportStrategyErrorMessage);
            }
        }
    }
}
