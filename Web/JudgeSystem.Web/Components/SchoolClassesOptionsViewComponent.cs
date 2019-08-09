using System.Linq;

using JudgeSystem.Services.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JudgeSystem.Web.Components
{
    [ViewComponent(Name = "SchoolClassesOptions")]
    public class SchoolClassesOptionsViewComponent : ViewComponent
    {
        private const int DefaultId = 0;
        private readonly ISchoolClassService schoolClassService;

        public SchoolClassesOptionsViewComponent(ISchoolClassService schoolClassService)
        {
            this.schoolClassService = schoolClassService;
        }

        public IViewComponentResult Invoke(int id = DefaultId)
        {
            var schoolClasses = schoolClassService.GetAllClasses()
                .Select(c => new SelectListItem
                {
                    Text = $"{c.ClassNumber} {c.ClassType}",
                    Value = c.Id.ToString(),
                    Selected = c.Id == id
                });

            return View(schoolClasses);
        }
    }
}
