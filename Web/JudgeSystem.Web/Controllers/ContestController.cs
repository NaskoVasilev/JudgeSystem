using JudgeSystem.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JudgeSystem.Web.Controllers
{
	public class ContestController : BaseController
	{
		private readonly IContestService contestService;

		public ContestController(IContestService contestService)
		{
			this.contestService = contestService;
		}

		public int GetNumberOfPages()
		{
			int pagesNumber = contestService.GetNumberOfPages();
			return pagesNumber;
		}
	}
}
