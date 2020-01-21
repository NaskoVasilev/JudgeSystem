using JudgeSystem.Common;

using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Controllers
{
    public class BaseController : Controller
    {
		public IActionResult ShowError(string errorMessage, string action, string conrtoller)
		{
			TempData[GlobalConstants.ErrorKey] = errorMessage;
			return RedirectToAction(action, conrtoller);
		}

		public IActionResult ShowError(string errorMessage, string action, string conrtoller, string area)
		{
			TempData[GlobalConstants.ErrorKey] = errorMessage;
			return RedirectToAction(action, conrtoller, new { area });
		}

		public IActionResult ShowError(string errorMessage, string action, string conrtoller, object routeValues)
		{
			TempData[GlobalConstants.ErrorKey] = errorMessage;
			return RedirectToAction(action, conrtoller, routeValues);
		}

		public IActionResult ShowInfo(string infoMessage, string action, string conrtoller)
		{
			TempData[GlobalConstants.InfoKey] = infoMessage;
			return RedirectToAction(action, conrtoller);
		}

        public IActionResult ShowInfo(string infoMessage, string action, object routeValues)
        {
            TempData[GlobalConstants.InfoKey] = infoMessage;
            return RedirectToAction(action, routeValues);
        }

        public IActionResult ShowInfo(string infoMessage, string action, string conrtoller, object routeValues)
		{
			TempData[GlobalConstants.InfoKey] = infoMessage;
			return RedirectToAction(action, conrtoller, routeValues);
		}
	}
}
