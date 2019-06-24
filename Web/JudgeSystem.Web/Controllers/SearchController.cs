using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Controllers
{
	public class SearchController : BaseController
	{
		public IActionResult Results(string keyword)
		{
			return this.View();
		}
	}
}
