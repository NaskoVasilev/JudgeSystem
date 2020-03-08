using System;

using JudgeSystem.Data.Models.Enums;

namespace JudgeSystem.Web.Utilites.ImportTests
{
    public class ParseTestsStartegyFactory
    {
        private const string UnsupportedTestsImportStrategyErrorMessage = "This tests import strategy is not supported!";

        public ParseTestsStrategy CreateStrategy(TestsImportStrategy strategy) => 
            strategy switch
        {
            TestsImportStrategy.Json => new ParseTestsFromJson(),
            TestsImportStrategy.Zip => new ParseTestsFromZip(),
            TestsImportStrategy.TestingProject => new ParseTestsFromTestingProject(),
            _ => throw new ArgumentException(UnsupportedTestsImportStrategyErrorMessage),
        };
    }
}
