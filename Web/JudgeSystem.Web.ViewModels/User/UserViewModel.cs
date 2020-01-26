using JudgeSystem.Data.Models;
using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.ViewModels.User
{
    public class UserViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }
    }
}
