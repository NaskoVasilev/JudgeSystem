namespace JudgeSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class BaseController : Controller
    {
		public IActionResult ShowError(string errorMessage, string action, string conrtoller)
		{
			TempData["error"] = errorMessage;
			return RedirectToAction(action, conrtoller);
		}

		public IActionResult ShowError(string errorMessage, string action, string conrtoller, string area)
		{
			TempData["error"] = errorMessage;
			return RedirectToAction(action, conrtoller, new { area });
		}
	}
}
