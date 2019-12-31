using JudgeSystem.Data.Models;

namespace JudgeSystem.Web.Tests.TestData
{
    public class RoleTestData
    {
        public const string Id = "role_id";

        public static ApplicationRole GetEntity(string roleName) => new ApplicationRole
        {
            Id = Id,
            Name = roleName,
            NormalizedName = roleName.ToUpper()
        };
    }
}
