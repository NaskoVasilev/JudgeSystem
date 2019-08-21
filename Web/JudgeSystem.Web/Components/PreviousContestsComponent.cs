﻿using System.Threading.Tasks;
using System.Collections.Generic;

using JudgeSystem.Services.Data;
using JudgeSystem.Web.ViewModels.Contest;

using Microsoft.AspNetCore.Mvc;

namespace JudgeSystem.Web.Components
{
	[ViewComponent(Name = "PreviousContests")]
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
			var previousContests = await Task.Run(() => 
            contestService.GetPreviousContests(PassedDaysFromContestEndTime));
			return View(previousContests);
		}
	}
}
