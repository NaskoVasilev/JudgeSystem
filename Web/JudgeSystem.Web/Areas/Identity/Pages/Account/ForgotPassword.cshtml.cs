using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Data.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JudgeSystem.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private const string ResetPasswordPageUrl = "/Account/ResetPassword";
        private const string ForgotPasswordConfirmationPageRoute = "./ForgotPasswordConfirmation";

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailSender emailSender;

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByEmailAsync(Input.Email);
                if (user == null || !user.EmailConfirmed)
                {
                    return RedirectToPage(ForgotPasswordConfirmationPageRoute);
                }

                string code = await userManager.GeneratePasswordResetTokenAsync(user);
                string callbackUrl = Url.Page(
                    ResetPasswordPageUrl,
                    pageHandler: null,
                    values: new { code },
                    protocol: Request.Scheme);

                string message = string.Format(GlobalConstants.PasswordResetMessage, HtmlEncoder.Default.Encode(callbackUrl));
                await emailSender.SendEmailAsync(Input.Email, GlobalConstants.ResetPasswordEmailSubject, message);

                return RedirectToPage(ForgotPasswordConfirmationPageRoute);
            }

            return Page();
        }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }
    }
}
