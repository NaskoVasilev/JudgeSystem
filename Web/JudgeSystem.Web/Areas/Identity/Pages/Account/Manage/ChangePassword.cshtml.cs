using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Data.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JudgeSystem.Web.Areas.Identity.Pages.Account.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public ChangePasswordModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var user = await this.userManager.GetUserAsync(this.User);

            var changePasswordResult = await this.userManager.ChangePasswordAsync(user, this.Input.OldPassword, this.Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }

                return this.Page();
            }

            await this.signInManager.RefreshSignInAsync(user);
            this.StatusMessage = InfoMessages.ChangeUserPasswordSuccessfully;

            return this.RedirectToPage();
        }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = ModelConstants.UserOldPasswordDisplayName)]
            public string OldPassword { get; set; }

            [Required]
            [StringLength(ModelConstants.UserPasswordMaxLength, MinimumLength = ModelConstants.UserPasswordMinLength)]
            [DataType(DataType.Password)]
            [Display(Name = ModelConstants.UserNewPasswordDisplayName)]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = ModelConstants.UserConfirmPasswordDisplayName)]
            [Compare(nameof(NewPassword))]
            public string ConfirmPassword { get; set; }
        }
    }
}
