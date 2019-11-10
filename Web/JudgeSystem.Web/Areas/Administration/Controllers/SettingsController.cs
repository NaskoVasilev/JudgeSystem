using System;
using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Web.InputModels.Settings;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Areas.Administration.Controllers
{
    public class SettingsController : AdministrationBaseController
    {
        private readonly UserManager<ApplicationUser> userManager;

        public SettingsController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [Authorize(Roles = GlobalConstants.OwnerRoleName)]
        public IActionResult AddAdministrator() => View();

        [Authorize(Roles = GlobalConstants.OwnerRoleName)]
        [HttpPost]
        public async Task<IActionResult> AddAdministrator(UserIdentityConfirmationInputModel model) =>
            await ManageUserRole(model, InfoMessages.AddAdministrator, GlobalConstants.AdministratorRoleName, addToRole: true);

        [Authorize(Roles = GlobalConstants.OwnerRoleName)]
        public IActionResult RemoveAdministrator() => View();

        [Authorize(Roles = GlobalConstants.OwnerRoleName)]
        [HttpPost]
        public async Task<IActionResult> RemoveAdministrator(UserIdentityConfirmationInputModel model) => 
            await ManageUserRole(model, InfoMessages.RemoveAdministrator, GlobalConstants.AdministratorRoleName, addToRole: false);

        [NonAction]
        private async Task<IActionResult> ManageUserRole(UserIdentityConfirmationInputModel model, string successMessage, string role, bool addToRole)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ApplicationUser currentUser = await userManager.GetUserAsync(User);
            if (!await userManager.CheckPasswordAsync(currentUser, model.ConfirmPassword))
            {
                ModelState.AddModelError(nameof(model.ConfirmPassword), ErrorMessages.InvalidPassword);
                return View(model);
            }

            ApplicationUser targetUser = await userManager.FindByNameAsync(model.Username);
            if (targetUser == null || targetUser.Name != model.Name || targetUser.Surname != model.Surname)
            {
                ModelState.AddModelError(string.Empty, ErrorMessages.UserNotFound);
                return View(model);
            }

            IdentityResult result = null;
            if (addToRole)
            {
                result = await userManager.AddToRoleAsync(targetUser, role);
            }
            else
            {
                result = await userManager.RemoveFromRoleAsync(targetUser, role);
            }

            if (result.Succeeded)
            {
                return ShowInfo(string.Format(successMessage, model.Username), "Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, string.Join(Environment.NewLine, result.Errors.Select(x => x.Description)));
                return View(model);
            }
        }
    }
}
