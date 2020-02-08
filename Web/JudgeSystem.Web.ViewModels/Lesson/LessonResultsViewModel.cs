using System.Collections.Generic;
using System.Linq;

namespace JudgeSystem.Web.ViewModels.Lesson
{
    public class LessonResultViewModel
    {
        public Dictionary<int, int> PointsByProblem { get; set; }

        public int Total => PointsByProblem.Values.Sum();

        public int GetPoints(int problemId)
        {
            PointsByProblem.TryGetValue(problemId, out int points);
            return points;
        }
    }
}
