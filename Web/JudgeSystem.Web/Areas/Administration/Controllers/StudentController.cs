namespace JudgeSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using JudgeSystem.Common;
    using JudgeSystem.Data.Models;
    using JudgeSystem.Data.Models.Enums;
    using JudgeSystem.Services.Data;
    using JudgeSystem.Services.Mapping;
    using JudgeSystem.Web.InputModels.Student;
    using JudgeSystem.Web.ViewModels.Student;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class StudentController : AdministrationBaseController
	{
		private readonly IStudentService studentService;
		private readonly ISchoolClassService schoolClassService;
        private readonly IEmailSender emailSender;

		public StudentController(
            IStudentService studentService, 
            ISchoolClassService schoolClassService,
            IEmailSender emailSender)
		{
			this.studentService = studentService;
			this.schoolClassService = schoolClassService;
            this.emailSender = emailSender;
        }

		public IActionResult Create()
		{
			ViewData["classes"] = GetSchoolClassesAsSelectListItem();
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(StudentCreateInputModel model)
		{
			if (!ModelState.IsValid)
			{
				ViewData["classes"] = GetSchoolClassesAsSelectListItem();
				return View(model);
			}

			string activationKey = Guid.NewGuid().ToString();
			await SendActivationEmail(activationKey, model.Email);

			Student student = model.To<Student>();
			student.ActivationKeyHash = activationKey;
			await studentService.Create(student);

			return Redirect("/");
		}

		public IActionResult StudentsByClass(int? classNumber, SchoolClassType? classType)
		{
			IEnumerable<StudentProfileViewModel> students = studentService.SearchStudentsByClass(classNumber, classType);
			return View(students);
		}

		public async Task<IActionResult> Edit(string id)
		{
			ViewData["classes"] = GetSchoolClassesAsSelectListItem();
			var model = await studentService.GetById<StudentEditInputModel>(id);
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(StudentEditInputModel model)
		{
			if(!ModelState.IsValid)
			{
				ViewData["classes"] = GetSchoolClassesAsSelectListItem();
				return View(model);
			}

			Student student = await studentService.UpdateAsync(model);
			SchoolClass schoolClass = await studentService.GetStudentClassAsync(student.Id);
			return RedirectToAction(nameof(StudentsByClass), 
				new { classNumber = schoolClass.ClassNumber, classType = schoolClass.ClassType });
		}

		public async Task<IActionResult> Delete(string id)
		{
			var model = await studentService.GetById<StudentProfileViewModel>(id);
			return View(model);
		}

		[HttpPost]
		[ActionName(nameof(Delete))]
		public async Task<IActionResult> DeletePost(string id)
		{
			Student student = await studentService.GetById(id);
			SchoolClass schoolClass = await studentService.GetStudentClassAsync(student.Id);
			await studentService.DeleteAsync(student);
			return RedirectToAction(nameof(StudentsByClass),
				new { classNumber = schoolClass.ClassNumber, classType = schoolClass.ClassType });
		}

		[NonAction]
		private async Task SendActivationEmail(string activationKey, string toAddress)
		{
			string subject = GlobalConstants.StudentProfileActivationEmailSubject;
			string message = await ReadEmailTemplateAsync();
			string activationKeyPlaceholder = "@{activationKey}";
			message = message.Replace(activationKeyPlaceholder, activationKey);
			
            await emailSender.SendEmailAsync(toAddress, subject, message);
		}

		[NonAction]
		private async Task<string> ReadEmailTemplateAsync()
		{
			string activationTemplateName = "StudentProfileActivation.html";
			string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", 
				GlobalConstants.TemplatesFolder, 
				GlobalConstants.EmailTemplatesFolder, 
				activationTemplateName);

			string temaplte = await System.IO.File.ReadAllTextAsync(path);
			return temaplte;
		}

		[NonAction]
		private IEnumerable<SelectListItem> GetSchoolClassesAsSelectListItem()
		{
			var schoolClasses = schoolClassService.GetAllClasses()
				.Select(c => new SelectListItem
				{
					Text = $"{c.ClassNumber} {c.ClassType}",
					Value = c.Id.ToString()
				});

			return schoolClasses;
		}
	}
}
