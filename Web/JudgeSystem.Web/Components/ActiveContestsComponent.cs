namespace JudgeSystem.Web.Components
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using JudgeSystem.Services.Data;
	using JudgeSystem.Web.ViewModels.Contest;
	using Microsoft.AspNetCore.Mvc;

	[ViewComponent(Name = "ActiveContest")]
	public class ActiveContestsComponent : ViewComponent
	{
		private readonly IContestService contestService;

		public ActiveContestsComponent(IContestService contestService)
		{
			this.contestService = contestService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			IEnumerable<ActiveContestViewModel> activeContests = await Task.Run(() => contestService.GetActiveContests());
			return View(activeContests);
		}
	}
}
