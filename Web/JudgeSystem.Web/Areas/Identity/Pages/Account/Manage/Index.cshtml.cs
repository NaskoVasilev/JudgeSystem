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
            ApplicationUser user = await userManager.GetUserAsync(User);
            Username = user.UserName;
            IsEmailConfirmed = user.EmailConfirmed;

            Input = new InputModel
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ApplicationUser user = await userManager.GetUserAsync(User);
            if (Input.Email != user.Email)
            {
                IdentityResult setEmailResult = await userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
                }
            }

            if (Input.PhoneNumber != user.PhoneNumber)
            {
                IdentityResult setPhoneResult = await userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
                }
            }

            await signInManager.RefreshSignInAsync(user);
            StatusMessage = InfoMessages.SuccessfullyUpdatedProfile;
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ApplicationUser user = await userManager.GetUserAsync(User);
            string code = await userManager.GenerateEmailConfirmationTokenAsync(user);

            string callbackUrl = Url.Page(
                ConfirmationPageUrl,
                pageHandler: null,
                values: new { userId = user.Id, code },
                protocol: Request.Scheme);

            string message = string.Format(GlobalConstants.EmailConfirmationMessage, HtmlEncoder.Default.Encode(callbackUrl));
            await emailSender.SendEmailAsync(user.Email, GlobalConstants.ConfirmEmailSubject, message);

            StatusMessage = InfoMessages.VerificationEmailSentMessage;
            return RedirectToPage();
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
