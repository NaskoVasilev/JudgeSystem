using System.ComponentModel.DataAnnotations;

using JudgeSystem.Common;

namespace JudgeSystem.Data.Models.Enums
{
    public enum TestsImportStrategy
    {
        [Display(Name = ModelConstants.TestJsonImportStrategyDisplayName)]
        Json = 1,
        [Display(Name = ModelConstants.TestZipImportStrategyDisplayName)]
        Zip = 2
    }
}
