using System.Collections.Generic;
using System.Linq;
using JudgeSystem.Common;
using JudgeSystem.Web.ViewModels.Student;

namespace JudgeSystem.Web.ViewModels.Contest
{
	public class ContestResultViewModel
	{
		public StudentBreifInfoViewModel Student { get; set; }

        public string UserId { get; set; }

        public Dictionary<int, int> PointsByProblem { get; set; }

		public int Total => PointsByProblem.Values.Sum();

        public int GetPoints(int problemId)
        {
            PointsByProblem.TryGetValue(problemId, out int points);
            return points;
        }
	}
}
