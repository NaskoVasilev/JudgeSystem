using System.Collections.Generic;
using System.Linq;

using JudgeSystem.Services.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JudgeSystem.Web.Components
{
    [ViewComponent(Name = "SchoolClassesOptions")]
    public class SchoolClassesOptionsViewComponent : ViewComponent
    {
        private readonly ISchoolClassService schoolClassService;

        public SchoolClassesOptionsViewComponent(ISchoolClassService schoolClassService)
        {
            this.schoolClassService = schoolClassService;
        }

        public IViewComponentResult Invoke(int id)
        {
            IEnumerable<SelectListItem> schoolClasses = schoolClassService.GetAllClasses()
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
