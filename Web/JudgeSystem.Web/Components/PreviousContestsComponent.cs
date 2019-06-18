using JudgeSystem.Services.Data;
using JudgeSystem.Web.ViewModels.Contest;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JudgeSystem.Web.Components
{
	[ViewComponent(Name = "PreviousContest")]
	public class PreviousContestsComponent : ViewComponent
	{
		private const int PassedDaysFromContestEndTime = 3;
		private readonly IContestService contestService;

		public PreviousContestsComponent(IContestService contestService)
		{
			this.contestService = contestService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			IEnumerable<PreviousContestViewModel> previousContests = await Task.Run(() => contestService.GetPreviousContests(PassedDaysFromContestEndTime));
			return View(previousContests);
		}
	}
}
