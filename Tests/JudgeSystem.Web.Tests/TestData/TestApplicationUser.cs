using JudgeSystem.Data.Models;

namespace JudgeSystem.Web.Tests.TestData
{
    public static class TestApplicationUser
    {
        public static string Id { get; set; } = "test_id";

        public static string Username { get; } = "test_username";

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
