using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Data.Models.Enums;
using JudgeSystem.Services;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.Dtos.SchoolClass;
using JudgeSystem.Web.Dtos.Student;
using JudgeSystem.Web.Infrastructure.Pagination;
using JudgeSystem.Web.InputModels.Student;
using JudgeSystem.Web.Utilites;
using JudgeSystem.Web.ViewModels.Student;

using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Areas.Administration.Controllers
{
    public class StudentController : AdministrationBaseController
	{
		private readonly IStudentService studentService;
		private readonly ISchoolClassService schoolClassService;
        private readonly IStudentProfileService studentProfileService;
        private readonly IPaginationService paginationService;
        private readonly IRouteBuilder routeBuilder;

        public StudentController(
            IStudentService studentService, 
            ISchoolClassService schoolClassService,
            IStudentProfileService studentProfileService,
            IPaginationService paginationService,
            IRouteBuilder routeBuilder)
		{
			this.studentService = studentService;
			this.schoolClassService = schoolClassService;
            this.studentProfileService = studentProfileService;
            this.paginationService = paginationService;
            this.routeBuilder = routeBuilder;
        }

        public IActionResult Create() => View();

        [ValidateAntiForgeryToken]
		[HttpPost]
		public async Task<IActionResult> Create(StudentCreateInputModel model)
		{
            if (studentService.ExistsByEmail(model.Email))
            {
                ModelState.AddModelError(nameof(model.Email), ErrorMessages.StudentWithTheSameEmailAlreadyExists);
            }
            if (studentService.ExistsByClassAndNumber(model.SchoolClassId, model.NumberInCalss))
            {
                string grade = await schoolClassService.GetGrade(model.SchoolClassId);
                ModelState.AddModelError(nameof(model.NumberInCalss), string.Format(ErrorMessages.StudentWithTheSameNumberInClassExists, grade));
            }

			if (!ModelState.IsValid)
			{
				return View(model);
			}

            string baseUrl = Utility.GetBaseUrl(HttpContext);
            string activationKey = await studentProfileService.SendActivationEmail(model.Email, baseUrl);
            StudentDto student = await studentService.Create(model, activationKey);
            return await RedirectToStudentsByClass(student.SchoolClassId);
		}

		public IActionResult StudentsByClass(int? classNumber, SchoolClassType? classType, int page = 1)
        {
            IEnumerable<StudentProfileViewModel> students = studentService.SearchStudentsByClass(classNumber, classType, page, GlobalConstants.StudentsPerPage);

            var paginationData = new PaginationData
            {
                CurrentPage = page,
                NumberOfPages = paginationService.CalculatePagesCount(studentService.StudentsByClassCount(classNumber, classType), GlobalConstants.StudentsPerPage),
                Url = routeBuilder.BuildStudentByClassRoute(classNumber, classType, nameof(StudentsByClass))
            };

            var model = new StudentsByClassViewModel
            {
                PaginationData = paginationData,
                Students = students
            };
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
		{
            StudentEditInputModel model = await studentService.GetById<StudentEditInputModel>(id);
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

            StudentDto student = await studentService.Update(model);
            return await RedirectToStudentsByClass(student.SchoolClassId);
		}

		public async Task<IActionResult> Delete(string id)
		{
            StudentProfileViewModel model = await studentService.GetById<StudentProfileViewModel>(id);
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
            SchoolClassDto schoolClass = await schoolClassService.GetById<SchoolClassDto>(schoolClassId);
            return RedirectToAction(nameof(StudentsByClass),
                new { classNumber = schoolClass.ClassNumber, classType = schoolClass.ClassType });
        }
    }
}
