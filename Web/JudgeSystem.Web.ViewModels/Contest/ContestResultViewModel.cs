using System.Collections.Generic;
using System.Linq;

using JudgeSystem.Web.ViewModels.Student;

namespace JudgeSystem.Web.ViewModels.Contest
{
	public class ContestResultViewModel
	{
		public StudentBreifInfoViewModel Student { get; set; }

		public Dictionary<int, int> PointsByProblem { get; set; }

		public int Total => PointsByProblem.Values.Sum();
	}
}
