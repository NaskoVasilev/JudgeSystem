using System.Linq;

using JudgeSystem.Services.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JudgeSystem.Web.Components
{
    [ViewComponent(Name = "CoursesSelectList")]
    public class CoursesSelectListViewComponent : ViewComponent
    {
        private readonly ICourseService courseService;

        public CoursesSelectListViewComponent(ICourseService courseService)
        {
            this.courseService = courseService;
        }

        public IViewComponentResult Invoke()
        {
            var courses = courseService.GetAllCourses()
                .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
            return View(courses);
        }
    }
}
