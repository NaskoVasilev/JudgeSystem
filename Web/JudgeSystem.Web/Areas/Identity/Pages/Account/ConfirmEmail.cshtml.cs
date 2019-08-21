using System;
using System.Threading.Tasks;
using JudgeSystem.Common;
using JudgeSystem.Data.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JudgeSystem.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ConfirmEmailModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            ApplicationUser user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load user with ID '{userId}'.");
            }

            IdentityResult result = await userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Error confirming email for user with ID '{userId}':");
            }

            TempData[GlobalConstants.InfoKey] = InfoMessages.SuccessfullyConfirmedEmail;
            return Redirect("/");
        }
    }
}
