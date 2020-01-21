using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Services.Data;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JudgeSystem.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IUserService userService;

        public LoginModel(SignInManager<ApplicationUser> signInManager, IUserService userService)
        {
            this.signInManager = signInManager;
            this.userService = userService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(Input.Username, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    if (userService.IsExistingUserWithNotConfirmedEmail(Input.Username))
                    {
                        ModelState.AddModelError(string.Empty, ErrorMessages.NotConfirmedEmail);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ErrorMessages.InvalidLoginAttempt);
                    }

                    return Page();
                }
            }

            return Page();
        }

        public class InputModel
        {
            [Required]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = ModelConstants.UserLoginRememeberMeDisplayName)]
            public bool RememberMe { get; set; }
        }
    }
}
