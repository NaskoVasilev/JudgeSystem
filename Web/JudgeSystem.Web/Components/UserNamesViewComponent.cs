using System.Threading.Tasks;

using JudgeSystem.Services.Data;
using JudgeSystem.Web.ViewModels.User;

using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Components
{
    public class UserNamesViewComponent: ViewComponent
    {
        private readonly IUserService userService;

        public UserNamesViewComponent(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userId)
        {
            UserNamesViewModel userNamesViewModel = await userService.GetUserNames(userId);
            return View(userNamesViewModel);
        }
    }
}
