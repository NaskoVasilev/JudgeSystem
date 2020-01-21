using System;

using JudgeSystem.Common;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index() => View();

        public IActionResult Documentation() => View();

        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            string cookie = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture));
            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddMonths(GlobalConstants.CultureCookieExpirationTimeInMonths)
            };

            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, cookie, cookieOptions);
            return Redirect(returnUrl);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View();
    }
}
