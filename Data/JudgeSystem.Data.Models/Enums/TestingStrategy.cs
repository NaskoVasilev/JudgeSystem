using System.ComponentModel.DataAnnotations;

using static JudgeSystem.Common.ModelConstants;

namespace JudgeSystem.Data.Models.Enums
{
    public enum TestingStrategy
    {
        [Display(Name = ProblemCheckOutputStrategyDisplayName)]
        CheckOutput = 1,
        [Display(Name = = ProblemRunAutomatedTestsStrategyDisplayName)]
        RunAutomatedTests = 2
    }
}
