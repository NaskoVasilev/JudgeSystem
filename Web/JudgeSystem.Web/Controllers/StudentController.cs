namespace JudgeSystem.Web.Controllers
{
	using System.Threading.Tasks;

	using JudgeSystem.Common;
	using JudgeSystem.Data.Models;
	using JudgeSystem.Services.Data;
	using JudgeSystem.Web.InputModels.Student;
	using JudgeSystem.Web.ViewModels.Student;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class StudentController : BaseController
	{
		private readonly IStudentService studentService;
		private readonly UserManager<ApplicationUser> userManager;

		public StudentController(
            IStudentService studentService, 
            UserManager<ApplicationUser> userManager)
		{
			this.studentService = studentService;
			this.userManager = userManager;
		}

		public IActionResult ActivateStudentProfile()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ActivateStudentProfile(StudentActivateProfileInputModel model)
		{
			if(!ModelState.IsValid)
			{
				return View(model);
			}

			Student student = await studentService.GetStudentProfileByActivationKey(model.ActivationKey);
			if(student == null)
			{
				ModelState.AddModelError(string.Empty, ErrorMessages.InvalidActivationKey);
				return View();
			}

			if (student.IsActivated)
			{
				ModelState.AddModelError(string.Empty, ErrorMessages.ActivatedStudentProfile);
				return View();
			}

			await studentService.SetStudentProfileAsActivated(student);
			ApplicationUser user = await userManager.GetUserAsync(this.User);
			user.StudentId = student.Id;
			await userManager.UpdateAsync(user);
			await userManager.AddToRoleAsync(user, GlobalConstants.StudentRoleName);

			return Redirect("/Identity/Account/Manage");
		}

		[Authorize(Roles = GlobalConstants.StudentRoleName)]
		public async Task<IActionResult> Profile()
		{
			ApplicationUser user = await userManager.GetUserAsync(this.User);
			if(user.StudentId == null)
			{
				this.ShowError(ErrorMessages.InvalidStudentProfile, "Home", "Controller");
				return Redirect("/");
			}

			StudentProfileViewModel model = await studentService.GetStudentInfo(user.StudentId);
			return View(model);
		}
	}
}
