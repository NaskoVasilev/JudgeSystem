using System.Collections.Generic;

namespace JudgeSystem.Web.ViewModels.Test
{
    public class ProblemTestsViewModel
    {
        public IEnumerable<TestViewModel> Tests { get; set; }

        public int LessonId { get; set; }
    }
}
