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
using Microsoft.Extensions.Logging;

namespace JudgeSystem.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private const string EmailConfirmationUrl = "/Account/ConfirmEmail";

        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<RegisterModel> logger;
        private readonly IEmailSender emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.logger = logger;
            this.emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public void OnGet(string returnUrl = null) => this.ReturnUrl = returnUrl;

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
				{
					UserName = Input.Username,
					Email = Input.Email,
					Name = Input.Name,
					Surname = Input.Surname,
				};

                IdentityResult result = await userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    logger.LogInformation("User created a new account with password.");

                    await userManager.AddToRoleAsync(user, GlobalConstants.BaseRoleName);
                    string code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    string callbackUrl = Url.Page(
                        EmailConfirmationUrl,
                        pageHandler: null,
                        values: new { userId = user.Id, code },
                        protocol: Request.Scheme);

                    string message = string.Format(GlobalConstants.EmailConfirmationMessage, HtmlEncoder.Default.Encode(callbackUrl));
                    await emailSender.SendEmailAsync(Input.Email, GlobalConstants.ConfirmEmailSubject, message);

                    TempData[GlobalConstants.InfoKey] = InfoMessages.VerificationEmailSentMessage;
                    return LocalRedirect(returnUrl);
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }

        public class InputModel
        {
			[StringLength(ModelConstants.UserFirstNameMaxLength, MinimumLength = ModelConstants.UserFirstNameMinLength)]
			[Required]
			public string Name { get; set; }

            [StringLength(ModelConstants.UserSurnameMaxLength, MinimumLength = ModelConstants.UserSurnameMinLength)]
            [Required]
			public string Surname { get; set; }

            [StringLength(ModelConstants.UserUsernameMaxLength, MinimumLength = ModelConstants.UserUsernameMinLength)]
            [Required]
			public string Username { get; set; }

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
        }
    }
}
