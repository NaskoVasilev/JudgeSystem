using System;
using System.Collections.Generic;
using System.Linq;

namespace JudgeSystem.Web.ViewModels.Practice
{
    public class PracticeResultViewModel
    {
        public string UserId { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public Dictionary<int, int> PointsByProblem { get; set; }

        public int Total => PointsByProblem.Values.Sum();

        public int GetPoints(int problemId)
        {
            PointsByProblem.TryGetValue(problemId, out int points);
            return points;
        }
    }
}
