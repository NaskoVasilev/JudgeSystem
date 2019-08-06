using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using JudgeSystem.Common;
using JudgeSystem.Data.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JudgeSystem.Web.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private const string ConfirmationPageUrl = "/Account/ConfirmEmail";
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailSender emailSender;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
        }

        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            this.Username = user.UserName;
            this.IsEmailConfirmed = user.EmailConfirmed;

            this.Input = new InputModel
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var user = await this.userManager.GetUserAsync(this.User);
            if (this.Input.Email != user.Email)
            {
                var setEmailResult = await this.userManager.SetEmailAsync(user, this.Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
                }
            }

            if (this.Input.PhoneNumber != user.PhoneNumber)
            {
                var setPhoneResult = await this.userManager.SetPhoneNumberAsync(user, this.Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
                }
            }

            await this.signInManager.RefreshSignInAsync(user);
            this.StatusMessage = InfoMessages.SuccessfullyUpdatedProfile;
            return this.RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var user = await this.userManager.GetUserAsync(this.User);
            var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);

            var callbackUrl = this.Url.Page(
                ConfirmationPageUrl,
                pageHandler: null,
                values: new { userId = user.Id, code },
                protocol: this.Request.Scheme);

            string message = string.Format(GlobalConstants.EmailConfirmationMessage, HtmlEncoder.Default.Encode(callbackUrl));
            await this.emailSender.SendEmailAsync(user.Email, GlobalConstants.ConfirmEmailSubject, message);

            this.StatusMessage = InfoMessages.VerificationEmailSentMessage;
            return this.RedirectToPage();
        }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Phone]
            [Display(Name = ModelConstants.UserPhoneNumberDisplayName)]
            public string PhoneNumber { get; set; }
        }
    }
}
