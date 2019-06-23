namespace JudgeSystem.Web.Controllers
{
	using JudgeSystem.Services.Data;

	using Microsoft.AspNetCore.Authorization;

	public class ContestController : BaseController
	{
		private readonly IContestService contestService;

		public ContestController(IContestService contestService)
		{
			this.contestService = contestService;
		}

		[Authorize]
		public int GetNumberOfPages()
		{
			int pagesNumber = contestService.GetNumberOfPages();
			return pagesNumber;
		}
	}
}
