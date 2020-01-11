using System.Collections.Generic;

using JudgeSystem.Data.Models;

namespace JudgeSystem.Web.Tests.TestData
{
    public class ApplicationUserTestData
    {
        public static IEnumerable<ApplicationUser> GenerateUsers()
        {
            for (int i = 0; i < 10; i++)
            {
                yield return new ApplicationUser
                {
                    Id = $"id_{i}",
                    UserName = $"username_{i}",
                    Surname = $"surname_{i}",
                    Name = $"name_{i}",
                    Email = $"email_{i}"
                };
            }
        }
    }
}
