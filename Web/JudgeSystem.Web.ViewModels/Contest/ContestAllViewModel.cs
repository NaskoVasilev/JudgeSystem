using System;
using System.Collections.Generic;
using System.Text;

namespace JudgeSystem.Web.ViewModels.Contest
{
	public class ContestAllViewModel
	{
		public IEnumerable<ContestViewModel> Contests { get; set; }

		public int NumberOfPages { get; set; }

		public int CurrentPage { get; set; }
	}
}
