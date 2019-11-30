using JudgeSystem.Data.Models;
using JudgeSystem.Data.Models.Enums;

namespace JudgeSystem.Web.Tests.TestData
{
    public class SchoolClassTestData
    {
        public static SchoolClass GetEntity() => new SchoolClass
        {
            Id = 1,
            ClassNumber = 12,
            ClassType = SchoolClassType.A
        };
    }
}
