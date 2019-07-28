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

        public async Task<IViewComponentResult> InvokeAsync(int lessonId)
        {
            IEnumerable<LessonProblemViewModel> lessons = await Task.Run(() => problemService.LessonProblems(lessonId));
            return View(lessons);
        }
    }
}
