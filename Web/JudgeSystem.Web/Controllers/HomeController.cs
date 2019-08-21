using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View();
    }
}
