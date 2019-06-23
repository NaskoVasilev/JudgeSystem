using JudgeSystem.Web.ViewModels.Problem;
using System.Collections.Generic;

namespace JudgeSystem.Web.ViewModels.Contest
{
	public class ContestAllResultsViewModel
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public List<ContestResultViewModel> ContestResults { get; set; }

		public List<ContestProblemViewModel> Problems { get; set; }
	}
}
