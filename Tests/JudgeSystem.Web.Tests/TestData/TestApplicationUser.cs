using JudgeSystem.Data.Models;

using MyTested.AspNetCore.Mvc;

namespace JudgeSystem.Web.Tests.TestData
{
    public static class TestApplicationUser
    {
        public static string Id { get; set; } = TestUser.Identifier;

        public static string Username { get; } = TestUser.Username;

        public static string Email { get; } = "test@email.com";

        public static string Name { get; } = "test_name";

        public static string Surname { get; } = "test_surname";

        public static ApplicationUser GetDefaultUser() => new ApplicationUser
        {
            Id = Id,
            UserName = Username,
            Email = Email,
            Name = Name,
            Surname = Surname
        };
    }
}
