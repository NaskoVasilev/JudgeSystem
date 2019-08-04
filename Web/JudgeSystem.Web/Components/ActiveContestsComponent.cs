using System.Collections.Generic;
using System.Threading.Tasks;

using JudgeSystem.Services.Data;
using JudgeSystem.Web.ViewModels.Contest;

using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Components
{
    [ViewComponent(Name = "ActiveContests")]
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
