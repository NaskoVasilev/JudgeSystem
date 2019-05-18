using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Areas.Administration.Controllers
{
	public class CourseController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
    }
}