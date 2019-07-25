namespace JudgeSystem.Web.Areas.Administration.Controllers
{
	using System;
	using System.Threading.Tasks;

	using JudgeSystem.Common;
	using JudgeSystem.Data.Models;
	using JudgeSystem.Services.Data;
	using JudgeSystem.Services.Mapping;
    using JudgeSystem.Web.Filters;
    using JudgeSystem.Web.InputModels.Course;

	using Microsoft.AspNetCore.Mvc;

	public class CourseController : AdministrationBaseController
    {
		private readonly ICourseService courseService;

		public CourseController(ICourseService courseService)
		{
			this.courseService = courseService;
		}

		public IActionResult Create()
        {
            return View();
        }

		[HttpPost]
		public async Task<IActionResult> Create(CourseInputModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			await courseService.Add(model);

			return RedirectToAction("All", "Course");
		}

		public async Task<IActionResult> Edit(int id)
		{
			Course course = await courseService.GetById(id);
			if(course == null)
			{
				TempData[GlobalConstants.ErrorKey] = string.Format(ErrorMessages.NotFoundEntityMessage, "course");
				return RedirectToAction("All", "Course");
			}

			CourseEditModel model = course.To<Course, CourseEditModel>();
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(CourseEditModel model)
		{
			if(!ModelState.IsValid)
			{
				return View(model);
			}

			try
			{
				await courseService.Updade(model);
			}
			catch (ArgumentException ex)
			{
				ViewData[GlobalConstants.ErrorKey] = ex.Message;
			}
			return RedirectToAction("All", "Course");
		}

        [EndpointExceptionFilter]
		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			 Course course = await courseService.GetById(id);
			if(course == null)
			{
				return BadRequest(string.Format(ErrorMessages.NotFoundEntityMessage, "course"));
			}

			await courseService.Delete(course);
			return Content(string.Format(InfoMessages.SuccessfullyDeletedMessage, course.Name));
		}
    }
}