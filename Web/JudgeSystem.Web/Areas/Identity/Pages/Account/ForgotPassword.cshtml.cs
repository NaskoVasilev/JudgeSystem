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
            if (this.ModelState.IsValid)
            {
                var user = await this.userManager.FindByEmailAsync(this.Input.Email);
                if (user == null || !user.EmailConfirmed)
                {
                    return this.RedirectToPage(ForgotPasswordConfirmationPageRoute);
                }

                var code = await this.userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = this.Url.Page(
                    ResetPasswordPageUrl,
                    pageHandler: null,
                    values: new { code },
                    protocol: this.Request.Scheme);

                string message = string.Format(GlobalConstants.PasswordResetMessage, HtmlEncoder.Default.Encode(callbackUrl));
                await this.emailSender.SendEmailAsync(Input.Email, GlobalConstants.ResetPasswordEmailSubject, message);

                return this.RedirectToPage(ForgotPasswordConfirmationPageRoute);
            }

            return this.Page();
        }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }
    }
}
