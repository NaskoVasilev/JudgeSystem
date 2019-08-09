﻿using System.Collections.Generic;
using System.Threading.Tasks;

using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.Dtos.SchoolClass;
using JudgeSystem.Web.Dtos.Student;
using JudgeSystem.Web.InputModels.Student;
using JudgeSystem.Web.ViewModels.Student;

using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Areas.Administration.Controllers
{
    public class StudentController : AdministrationBaseController
	{
		private readonly IStudentService studentService;
		private readonly ISchoolClassService schoolClassService;
        private readonly IStudentProfileService studentProfileService;

		public StudentController(
            IStudentService studentService, 
            ISchoolClassService schoolClassService,
            IStudentProfileService studentProfileService)
		{
			this.studentService = studentService;
			this.schoolClassService = schoolClassService;
            this.studentProfileService = studentProfileService;
        }

		public IActionResult Create()
		{
			return View();
		}

        [ValidateAntiForgeryToken]
		[HttpPost]
		public async Task<IActionResult> Create(StudentCreateInputModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

            string activationKey = await studentProfileService.SendActivationEmail(model.Email);
			var student = await studentService.Create(model, activationKey);
            return await RedirectToStudentsByClass(student.SchoolClassId);
		}

		public IActionResult StudentsByClass(int? classNumber, SchoolClassType? classType)
		{
			IEnumerable<StudentProfileViewModel> students = studentService.SearchStudentsByClass(classNumber, classType);
			return View(students);
		}

		public async Task<IActionResult> Edit(string id)
		{
			var model = await studentService.GetById<StudentEditInputModel>(id);
			return View(model);
		}

        [ValidateAntiForgeryToken]
		[HttpPost]
		public async Task<IActionResult> Edit(StudentEditInputModel model)
		{
			if(!ModelState.IsValid)
			{
				return View(model);
			}

			var student = await studentService.Update(model);
            return await RedirectToStudentsByClass(student.SchoolClassId);
		}

		public async Task<IActionResult> Delete(string id)
		{
			var model = await studentService.GetById<StudentProfileViewModel>(id);
			return View(model);
		}

		[HttpPost]
        [ValidateAntiForgeryToken]
		[ActionName(nameof(Delete))]
		public async Task<IActionResult> DeletePost(string id)
        {
            StudentDto student = await studentService.Delete(id);
            return await RedirectToStudentsByClass(student.SchoolClassId);
        }

        private async Task<IActionResult> RedirectToStudentsByClass(int schoolClassId)
        {
            var schoolClass = await schoolClassService.GetById<SchoolClassDto>(schoolClassId);
            return RedirectToAction(nameof(StudentsByClass),
                new { classNumber = schoolClass.ClassNumber, classType = schoolClass.ClassType });
        }
    }
}
