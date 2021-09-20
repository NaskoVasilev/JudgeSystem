using System.Linq;

namespace JudgeSystem.Common.Helpers
{
    public static class UserHelper
    {
        public static string GetUsernameFromEmail(string email) => email.Split('@').First();
    }
}
