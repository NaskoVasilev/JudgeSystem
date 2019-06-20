namespace JudgeSystem.Web.Areas.Administration.Controllers
{
	using JudgeSystem.Web.InputModels.Student;

	using Microsoft.AspNetCore.Mvc;

	public class StudentController : AdministrationBaseController
	{
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(StudentCreateInputModel model)
		{
			if(!ModelState.IsValid)
			{
				return View(model);
			}

			//TODO: redirect to all students
			return Redirect("/");
		}
	}
}
