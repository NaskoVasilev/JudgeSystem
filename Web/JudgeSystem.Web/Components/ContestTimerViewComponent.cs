using System.Threading.Tasks;

using JudgeSystem.Services;
using JudgeSystem.Services.Data;
using JudgeSystem.Web.Dtos.Common;
using JudgeSystem.Web.Dtos.Contest;

using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Components
{
	public class ContestTimerViewComponent : ViewComponent
	{
		private readonly IContestService contestService;
		private readonly IEstimator estimator;

		public ContestTimerViewComponent(IContestService contestService, IEstimator estimator)
		{
			this.contestService = contestService;
			this.estimator = estimator;
		}

		public async Task<IViewComponentResult> InvokeAsync(int contestId)
		{
			ContestStartEndTimeDto contest = await contestService.GetById<ContestStartEndTimeDto>(contestId);
			TimeRemainingDto model = estimator.CalculateRemainingTime(contest.EndTime);
            model.ContestName = contest.Name;
			return View(model);
		}
	}
}
