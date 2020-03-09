using System.ComponentModel.DataAnnotations;

using static JudgeSystem.Common.ModelConstants;

namespace JudgeSystem.Data.Models.Enums
{
    public enum TestsImportStrategy
    {
        [Display(Name = TestJsonImportStrategyDisplayName)]
        Json = 1,
        [Display(Name = TestZipImportStrategyDisplayName)]
        Zip = 2,
        [Display(Name = TestingProjectStrategyDisplayName)]
        TestingProject = 3
    }
}
