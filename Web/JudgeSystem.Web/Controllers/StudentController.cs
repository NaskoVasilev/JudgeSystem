﻿using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Data.Models;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.Dtos.Student;
using JudgeSystem.Web.InputModels.Student;
using JudgeSystem.Web.ViewModels.Student;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Controllers
{
    [Authorize]
	public class StudentController : BaseController
	{
		private readonly IStudentService studentService;
		private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public StudentController(
            IStudentService studentService, 
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
		{
			this.studentService = studentService;
			this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult ActivateStudentProfile() => View();

        [HttpPost]
		public async Task<IActionResult> ActivateStudentProfile(StudentActivateProfileInputModel model)
		{
			if(!ModelState.IsValid)
			{
				return View(model);
			}

            StudentDto student = await studentService.GetStudentProfileByActivationKey(model.ActivationKey);
			if(student == null)
			{
				ModelState.AddModelError(nameof(StudentActivateProfileInputModel.ActivationKey), ErrorMessages.InvalidActivationKey);
				return View(model);
			}
			if (student.IsActivated)
			{
				ModelState.AddModelError(string.Empty, ErrorMessages.ActivatedStudentProfile);
				return View(model);
			}

			await studentService.SetStudentProfileAsActivated(student.Id);

            ApplicationUser user = await userManager.GetUserAsync(User);
			user.StudentId = student.Id;
			await userManager.UpdateAsync(user);
			await userManager.AddToRoleAsync(user, GlobalConstants.StudentRoleName);
            await signInManager.SignInAsync(user, isPersistent: false);

			return Redirect("/Identity/Account/Manage");
		}

		[Authorize(Roles = GlobalConstants.StudentRoleName)]
		public async Task<IActionResult> Profile()
		{
            ApplicationUser user = await userManager.GetUserAsync(User);
			if(user.StudentId == null)
			{
                return ShowError(ErrorMessages.InvalidStudentProfile, nameof(HomeController.Index), "Home");
			}

			StudentProfileViewModel model = await studentService.GetById<StudentProfileViewModel>(user.StudentId);
			return View(model);
		}
	}
}
