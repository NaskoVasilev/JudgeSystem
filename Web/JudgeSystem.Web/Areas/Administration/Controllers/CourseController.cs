using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.Filters;
using JudgeSystem.Web.InputModels.Course;

using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Areas.Administration.Controllers
{
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

        [ValidateAntiForgeryToken]
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

        public IActionResult Edit(int id)
        {
            var course = courseService.GetById<CourseInputModel>(id);
            return View(course);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(CourseEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await courseService.Updade(model);

            return RedirectToAction("All", "Course");
        }

        [EndpointExceptionFilter]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var course =  await courseService.Delete(id);
            return Content(string.Format(InfoMessages.SuccessfullyDeletedMessage, course.Name));
        }
    }
}