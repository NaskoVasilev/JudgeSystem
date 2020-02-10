using System;

using JudgeSystem.Data.Models.Enums;

namespace JudgeSystem.Web.Utilites.ImportTests
{
    public class ParseTestsStartegyFactory
    {
        private const string UnsupportedTestsImportStrategyErrorMessage = "This tests import strategy is not supported!";

        public ParseTestsStrategy CreateStrategy(TestsImportStrategy strategy)
        {
            switch (strategy)
            {
                case TestsImportStrategy.Json:
                    return new ParseTestsFromJson();
                case TestsImportStrategy.Zip:
                    return new ParseTestsFromZip();
                default:
                    throw new ArgumentException(UnsupportedTestsImportStrategyErrorMessage);
            }
        }
    }
}
