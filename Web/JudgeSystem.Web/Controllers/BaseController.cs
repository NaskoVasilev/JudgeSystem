namespace JudgeSystem.Web.Controllers
{
    using JudgeSystem.Common;
    using JudgeSystem.Common.Exceptions;

    using Microsoft.AspNetCore.Mvc;

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

		public IActionResult ShowInfo(string infoMessage, string action, string conrtoller, object routeValues)
		{
			TempData[GlobalConstants.InfoKey] = infoMessage;
			return RedirectToAction(action, conrtoller, routeValues);
		}

		public void ThrowEntityNotFoundException(string entityName)
		{
			throw new EntityNotFoundException(entityName);
		}
	}
}
