using JudgeSystem.Services.Data;
using JudgeSystem.Web.ViewModels.User;
using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Areas.Administration.Controllers
{
    public class UserController : AdministrationBaseController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult Results(string userId)
        {
            UserResultsViewModel userResults = new UserResultsViewModel
            {
                ContestResults = userService.GetContestResults(userId),
                PracticeResults = userService.GetPracticetResults(userId)
            };
            return View(userResults);
        }
    }
}
