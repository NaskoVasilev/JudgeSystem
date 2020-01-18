using System.Threading.Tasks;

using JudgeSystem.Services.Data;

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
            var activeContests = await Task.Run(() => contestService.GetActiveContests());
            return View(activeContests);
        }
	}
}
