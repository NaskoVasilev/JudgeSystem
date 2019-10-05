using JudgeSystem.Services.Mapping;

namespace JudgeSystem.Web.ViewModels.User
{
    public class UserNamesViewModel : IMapFrom<Data.Models.ApplicationUser>
    {
        public string UserName { get; set; }

        public string StudentFullName { get; set; }
    }
}
