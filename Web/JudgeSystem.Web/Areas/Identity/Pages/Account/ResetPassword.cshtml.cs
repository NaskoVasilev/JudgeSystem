using System;
using System.ComponentModel.DataAnnotations;
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
    public class ResetPasswordModel : PageModel
    {
        private const string LoginPageRoute = "./Login";

        private readonly UserManager<ApplicationUser> userManager;

        public ResetPasswordModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IActionResult OnGet(string code = null)
        {
            if (code == null)
            {
                throw new ArgumentException("A code must be supplied for password reset.");
            }
            else
            {
                this.Input = new InputModel
                {
                    Code = code,
                };

                return this.Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var user = await this.userManager.FindByEmailAsync(this.Input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return this.RedirectToPage(LoginPageRoute);
            }

            var result = await this.userManager.ResetPasswordAsync(user, this.Input.Code, this.Input.Password);
            if (result.Succeeded)
            {
                return this.RedirectToPage(LoginPageRoute);
            }

            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }

            return this.Page();
        }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(ModelConstants.UserPasswordMaxLength, MinimumLength = ModelConstants.UserPasswordMinLength)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = ModelConstants.UserConfirmPasswordDisplayName)]
            [Compare(nameof(Password))]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
        }
    }
}
