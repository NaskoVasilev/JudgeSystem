namespace JudgeSystem.Web.Components
{
    using System.Collections.Generic;

    using JudgeSystem.Services.Data;
    using JudgeSystem.Web.ViewModels.User;

    using Microsoft.AspNetCore.Mvc;

    public class UserExamsViewComponent : ViewComponent
	{
		private readonly IUserService userService;

		public UserExamsViewComponent(IUserService userService)
		{
			this.userService = userService;
		}

		public IViewComponentResult Invoke(string userId)
		{
			IEnumerable<UserCompeteResultViewModel> examResults = userService.GetUserExamResults(userId);
			return View(examResults);
		}
	}
}
