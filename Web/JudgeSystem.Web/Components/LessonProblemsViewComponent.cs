using JudgeSystem.Services.Data;
using JudgeSystem.Web.ViewModels.Problem;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JudgeSystem.Web.Components
{
    [ViewComponent(Name = "LessonProblems")]
    public class LessonProblemsViewComponent : ViewComponent
    {
        private readonly IProblemService problemService;

        public LessonProblemsViewComponent(IProblemService problemService)
        {
            this.problemService = problemService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int lessonId, string url)
        {
            IEnumerable<LessonProblemViewModel> problems = await Task.Run(() => problemService.LessonProblems(lessonId));
            var model = new LessonProblemsViewComponentModel { Problems = problems, UrlPlaceholder = url };
            return View(model);
        }
    }
}
