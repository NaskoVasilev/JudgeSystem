using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using JudgeSystem.Services;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.InputModels.User;
using JudgeSystem.Web.ViewModels.User;
using JudgeSystem.Common.Extensions;
using JudgeSystem.Services.Models.Users;
using JudgeSystem.Common;
using JudgeSystem.Services.Validations.Contracts;
using JudgeSystem.Common.Models;
using JudgeSystem.Web.Infrastructure.Extensions;
using System.Linq;

namespace JudgeSystem.Web.Areas.Administration.Controllers
{
    public class UserController : AdministrationBaseController
    {
        private readonly IUserService userService;
        private readonly IUserValidationService userValidationService;
        private readonly IValidationService validationService;

        public UserController(
            IUserService userService,
            IUserValidationService userValidationService,
            IValidationService validationService)
        {
            this.userService = userService;
            this.userValidationService = userValidationService;
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

        [HttpGet]
        public IActionResult Import() => View();

        [HttpPost]
        public async Task<IActionResult> Import(ImportUsersInputModel model)
        {
            if (!validationService.IsValidFileExtension(model.File.FileName, GlobalConstants.JsonFileExtension))
            {
                ModelState.AddModelError(nameof(ImportUsersInputModel.File), "You should upload a valid JSON file!");
                return View();
            }

            using (Stream stream = model.File.OpenReadStream())
            {
                string json = await stream.ReadToEndAsync();
                IList<UserImportServiceModel> users = json.FromJson<IList<UserImportServiceModel>>();

                Result validationResult = userValidationService.ValidateUsersForImport(users);

                if (!validationResult.Succeeded)
                {
                    ModelState.AddErrors(nameof(ImportUsersInputModel.File), validationResult.Errors);
                    return View();
                }

                await userService.ImportAsync(users);

                string infoMessage = $"{users.Count} users was successfully imported!";
                return ShowInfo(infoMessage, nameof(All));
            }
        }
    }
}
