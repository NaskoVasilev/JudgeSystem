using System.Collections.Generic;
using System.Linq;
using JudgeSystem.Web.Infrastructure.Pagination;
using JudgeSystem.Web.ViewModels.Problem;

namespace JudgeSystem.Web.ViewModels.Practice
{
    public class PracticeAllResultsViewModel
    {
        public int Id { get; set; }

        public string LessonName { get; set; }

        public List<PracticeResultViewModel> PracticeResults { get; set; }

        public List<PracticeProblemViewModel> Problems { get; set; }

        public int MaxPoints => Problems.Sum(x => x.MaxPoints);

        public PaginationData PaginationData { get; set; }
    }
}
