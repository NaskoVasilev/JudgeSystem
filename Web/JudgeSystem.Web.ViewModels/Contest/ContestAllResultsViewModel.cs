using System.Linq;
using System.Collections.Generic;

using JudgeSystem.Web.Infrastructure.Pagination;
using JudgeSystem.Web.ViewModels.Problem;

namespace JudgeSystem.Web.ViewModels.Contest
{
	public class ContestAllResultsViewModel
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public List<ContestResultViewModel> ContestResults { get; set; }

		public List<ContestProblemViewModel> Problems { get; set; }

        public PaginationData PaginationData { get; set; }

        public int[] ProblemIds => Problems.Select(p => p.Id).ToArray();

        public int MaxPoints => Problems.Sum(x => x.MaxPoints);

    }
}
