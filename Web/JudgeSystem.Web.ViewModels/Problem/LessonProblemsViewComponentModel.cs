using System.Collections.Generic;

namespace JudgeSystem.Web.ViewModels.Problem
{
    public class LessonProblemsViewComponentModel
    {
        public IEnumerable<LessonProblemViewModel> Problems { get; set; }

        public string UrlPlaceholder { get; set; }
    }
}
