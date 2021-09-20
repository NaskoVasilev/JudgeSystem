using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using JudgeSystem.Services;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.InputModels.User;
using JudgeSystem.Web.ViewModels.User;
using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Areas.Administration.Controllers
{
    public class UserController : AdministrationBaseController
    {
        private readonly IUserService userService;
        private readonly IValidationService validationService;

        public UserController(IUserService userService, IValidationService validationService)
        {
            this.userService = userService;
            this.validationService = validationService;
        }

        public IActionResult Results(string userId)
        {
            var userResults = new UserResultsViewModel
            {
                ContestResults = userService.GetContestResults(userId),
                PracticeResults = userService.GetPracticeResults(userId),
                UserId = userId
            };

            return View(userResults);
        }

        public IActionResult All()
        {
            IEnumerable<UserViewModel> users = userService.All();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Import(ImportUsersInputModel model)
        {
            if (!validationService.IsValidFileExtension(model.File.FileName))
            {
                ModelState.AddModelError(nameof(ImportUsersInputModel.File), "You should upload a valid JSON file!");
            }
        }
    }
}
