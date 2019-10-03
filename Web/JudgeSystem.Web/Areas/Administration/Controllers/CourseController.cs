using System.Threading.Tasks;

using JudgeSystem.Common;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.Filters;
using JudgeSystem.Web.InputModels.Course;
using JudgeSystem.Web.ViewModels.Course;

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

        public IActionResult Create() => View();

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
            CourseEditModel course = courseService.GetById<CourseEditModel>(id);
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

        public IActionResult Delete(int id)
        {
            CourseViewModel course = courseService.GetById<CourseViewModel>(id);
            return View(course);
        }

        [HttpPost]
        [ActionName(nameof(Delete))]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            await courseService.Delete(id);
            return RedirectToAction("All", "Course");
        }
    }
}
