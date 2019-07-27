using System.Collections.Generic;

namespace JudgeSystem.Web.ViewModels.Problem
{
    public class ProblemAllViewModel
    {
        public IEnumerable<LessonProblemViewModel> Problems { get; set; }

        public int LesosnId { get; set; }

        public int PracticeId { get; set; }
    }
}
